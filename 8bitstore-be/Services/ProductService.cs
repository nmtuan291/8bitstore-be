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
using StackExchange.Redis;

namespace _8bitstore_be.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IConnectionMultiplexer _redis;
        public ProductService(IProductRepository productRepository, IConnectionMultiplexer redis)
        {
            _productRepository = productRepository;
            _redis = redis;
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
            
            var result = new PaginatedResult()
            {
                Products = filtered
                    .Skip((request.Page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(p => new ProductDto
                    {
                        ProductId = p.ProductID,
                        ProductName = p.ProductName,
                        Price = p.Price,
                        Platform = p.Platform,
                        Type = p.Type,
                        Genre = p.Genre,
                        Description = p.Description,
                        ImportDate = p.ImportDate,
                        ImgUrl = p.ImgUrl,
                        Manufacturer = p.Manufacturer,
                        StockNum = p.StockNum
                    }).ToList(),
                PageSize = pageSize,
                CurrentPage = request.Page,
                TotalPages = totalPages
            };

            return result;
        }
        
        public async Task<IEnumerable<ProductDto>> GetAllProductAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(p => new ProductDto()
            {
                ProductId = p.ProductID,
                ProductName = p.ProductName,
                Description = p.Description,
                Price = p.Price,
                ImgUrl = p.ImgUrl,
                ImportDate = p.ImportDate,
                Genre = p.Genre,
                Manufacturer = p.Manufacturer,
                StockNum = p.StockNum
            });
        }
    
        public async Task<ProductDto> GetProductAsync(string productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                throw new KeyNotFoundException("The product cannot be found");
            return new ProductDto
            {
                ProductId = product.ProductID,
                ProductName = product.ProductName,
                Price = product.Price,
                Platform = product.Platform,
                Type = product.Type,
                Genre = product.Genre,
                Description = product.Description,
                ImportDate = product.ImportDate,
                ImgUrl = product.ImgUrl,
                Manufacturer = product.Manufacturer,
                StockNum = product.StockNum
            };
        }

        public async Task<bool> AddProductAsync(ProductDto product)
        {
            try
            {
                if (product == null || string.IsNullOrEmpty(product.ProductName) || product.Price < 0)
                    return false;

                Product newProduct = new Product()
                {
                    ProductID = product.ProductId,
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
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<string>> GetSuggestionAsync(string query)
        {
            var db = _redis.GetDatabase();
            query = query.ToLower();
            var productName = await db.SortedSetRangeByValueAsync("products", query, query + char.MaxValue);
            return productName.Select(p => p.ToString());
        }
    }
}
