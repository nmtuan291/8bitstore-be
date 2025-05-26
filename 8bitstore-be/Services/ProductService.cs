using System.Linq;
using _8bitstore_be.Data;
using _8bitstore_be.DTO.Product;
using _8bitstore_be.Interfaces;
using _8bitstore_be.Models;
using Microsoft.EntityFrameworkCore;

namespace _8bitstore_be.Services
{
    public class ProductService: IProductService
    {
        private readonly _8bitstoreContext _context;
        public ProductService(_8bitstoreContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync(ProductRequest request)
        {
            IQueryable<Product> products = _context.Products;
            if (!string.IsNullOrEmpty(request.ProductName))
            {
                products = products.Where(p => p.ProductName == request.ProductName);
            }

            if (request.MinPrice.HasValue)
            {
                products = products.Where(p => p.Price >= request.MinPrice);
            }

            if (request.MaxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= request.MaxPrice);
            }

            if (request.Genres != null)
            {
                products = products.Where(p => p.Genre.Any(g => request.Genres.Contains(g)));
            }

            if (request.Manufacturer != null)
            {
                products = products.Where(p => request.Manufacturer.Contains(p.Manufacturer));
            }

            if (request.Type != null)
            {
                products = products.Where(p => request.Type.Contains(p.Type));
            } 

            if (request.Platforms != null)
            {
                products = products.Where(p => p.Platform.Any(plaform => request.Platforms.Contains(plaform)));
            }

            if (request.SortByName.HasValue && request.SortByName != 0)
            {
                products = products.OrderBy(p => p.ProductName);
            }

            if (request.SortByPrice.HasValue && request.SortByPrice != 0)
            {
                products = products.OrderBy(p => p.Price);
            }

            if (request.SortByDate.HasValue && request.SortByDate != 0)
            {
                products = products.OrderBy(p => p.ImportDate);
            }

            List<Product> productList = await products.ToListAsync();

            IEnumerable<ProductDto> productDtos = productList.Select(p => new ProductDto
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
            });

            return productDtos;
        }

        public async Task<ProductDto> GetProductAsync(string productId)
        {
            var product = await (from p in _context.Products
                                 where p.ProductID == productId
                                 select p).FirstOrDefaultAsync();

            if (product == null)
            {
                throw new KeyNotFoundException("The product cannot be found");
            }

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

            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<string>> GetSuggestionAsync(string query)
        {
            var productNames = await _context.Products
                   .Where(p => p.ProductName.Contains(query))
                   .Select(p => p.ProductName)
                   .Take(10)
                   .ToListAsync();

            return productNames;
        }
    }
}
