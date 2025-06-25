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

namespace _8bitstore_be.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync(ProductRequest request)
        {
            var products = await _productRepository.GetAllAsync();
            var filtered = products.AsQueryable();

            if (!string.IsNullOrEmpty(request.ProductName))
                filtered = filtered.Where(p => p.ProductName == request.ProductName);
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
                filtered = filtered.OrderBy(p => p.ImportDate);

            return filtered.Select(p => new ProductDto
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
            }).ToList();
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

        public async Task AddProductAsync(ProductDto product)
        {
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
            };
            await _productRepository.AddAsync(newProduct);
            await _productRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<string>> GetSuggestionAsync(string query)
        {
            var products = await _productRepository.GetProductsByNameAsync(query);
            return products.Select(p => p.ProductName).Take(10);
        }
    }
}
