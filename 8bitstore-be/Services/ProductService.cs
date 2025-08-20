using System.Linq;
using _8bitstore_be.Data;
using _8bitstore_be.DTO.Product;
using _8bitstore_be.Interfaces.Services;
using _8bitstore_be.Interfaces.Repositories;
using _8bitstore_be.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _8bitstore_be.Exceptions;
using StackExchange.Redis;

namespace _8bitstore_be.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IConnectionMultiplexer _redis;
        private readonly ILogger<ProductService> _logger;
        public ProductService(IProductRepository productRepository, IConnectionMultiplexer redis, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _redis = redis;
            _logger = logger;
        }

        public async Task<PaginatedResult> GetProductsAsync(ProductRequest request)
        {
            var products = await _productRepository.GetAllAsync();
            var filtered = products.AsQueryable();

            if (!string.IsNullOrEmpty(request.ProductName))
                filtered = filtered.Where(p => p.ProductName.ToLower().Contains(request.ProductName.ToLower()));
            if (request.MinPrice.HasValue)
                filtered = filtered.Where(p => p.Price >= request.MinPrice);
            if (request.MaxPrice.HasValue)
                filtered = filtered.Where(p => p.Price <= request.MaxPrice);
            if (request.Genres != null)
                filtered = filtered.Where(p => p.Genre.Any(g => request.Genres.Contains(g)));
            if (request.Manufacturer != null)
                filtered = filtered.Where(p => request.Manufacturer.Contains(p.Manufacturer));
            if (request.Type != null)
                filtered = filtered.Where(p => request.Type.Contains(p.Type));
            if (request.Platforms != null)
                filtered = filtered.Where(p => p.Platform.Any(platform => request.Platforms.Contains(platform)));
            if (request.SortByName.HasValue && request.SortByName != 0)
                filtered = filtered.OrderBy(p => p.ProductName);
            if (request.SortByPrice.HasValue && request.SortByPrice != 0)
                filtered = filtered.OrderBy(p => p.Price);
            if (request.SortByDate.HasValue && request.SortByDate != 0)
                filtered = filtered.OrderByDescending(p => p.ImportDate);
            if (request.SortByWeeklySales.HasValue && request.SortByWeeklySales != 0)
                filtered = filtered.OrderByDescending(p => p.WeeklySales);
            if (request.Top.HasValue)
                filtered = filtered.Take(request.Top.Value);
            
            int pageSize = 10;
            int totalCount = filtered.Count();
            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            
            var pagedProducts = filtered
                .Skip((request.Page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            
            var result = new PaginatedResult()
            {
                Products = pagedProducts.Select(p => new ProductDto
                {
                    ProductId = p.ProductID,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Platform = p.Platform?.ToList() ?? new List<string>(),
                    Type = p.Type,
                    Genre = p.Genre?.ToList() ?? new List<string>(),
                    Description = p.Description,
                    ImportDate = p.ImportDate,
                    ImgUrl = p.ImgUrl?.ToList() ??  new List<string>(),
                    Manufacturer = p.Manufacturer,
                    StockNum = p.StockNum
                }).ToList(),
                PageSize = pageSize,
                CurrentPage = request.Page,
                TotalPages = totalPages
            };

            return result;
        }
        
        public async Task<List<ProductDto>> GetAllProductAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(p => new ProductDto()
            {
                ProductId = p.ProductID,
                ProductName = p.ProductName,
                Description = p.Description,
                Price = p.Price,
                ImgUrl = p.ImgUrl?.ToList() ?? new List<string>(),
                ImportDate = p.ImportDate,
                Genre = p.Genre?.ToList() ?? new List<string>(),
                Manufacturer = p.Manufacturer,
                StockNum = p.StockNum
            }).ToList();
        }
    
        public async Task<ProductDto> GetProductAsync(string productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            
            if (product == null)
                throw new ProductNotFoundException(productId); 
            
            return new ProductDto
            {
                ProductId = product.ProductID,
                ProductName = product.ProductName,
                Price = product.Price,
                Platform = product.Platform?.ToList() ?? new List<string>(),
                Type = product.Type,
                Genre = product.Genre?.ToList() ??  new List<string>(),
                Description = product.Description,
                ImportDate = product.ImportDate,
                ImgUrl = product.ImgUrl?.ToList() ??  new List<string>(),
                Manufacturer = product.Manufacturer,
                StockNum = product.StockNum
            };
        }

        public async Task AddProductAsync(ProductDto product)
        {
            if (product.Price < 0)
                throw new ProductPriceException("Price cannot be negative");

            Product newProduct = new Product()
            {
                ProductID = product.ProductId ?? "",
                ProductName = product.ProductName,
                Price = product.Price,
                Platform = product.Platform,
                Type = product.Type,
                Genre = product.Genre,
                Manufacturer = product.Manufacturer,
                Description = product.Description,
                ImportDate = DateTime.UtcNow,
                ImgUrl = product.ImgUrl,
                StockNum = product.StockNum,
                WeeklySales = 0
            };
            var db = _redis.GetDatabase();
            await db.SortedSetAddAsync("products", newProduct.ProductName, 0);
                
            await _productRepository.AddAsync(newProduct);
            await _productRepository.SaveChangesAsync();
        }

        public async Task<List<string>> GetSuggestionAsync(string query)
        {
            var db = _redis.GetDatabase();
            query = query.ToLower();
            var productName = await db.SortedSetRangeByValueAsync("products", query, query + char.MaxValue);
            return productName.Select(p => p.ToString()).ToList();
        }
    }
}
