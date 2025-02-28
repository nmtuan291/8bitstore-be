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

        public async Task<IEnumerable<ProductDto>> GetProductsAsync(int? sortByName, int? sortByPrice, int? sortByDate)
        {
            IQueryable<Product> products = from product
                                           in _context.Products
                                           select product;

            if (sortByName.HasValue && sortByName != 0)
            {
                products.OrderBy(p => p.ProductName);
            }

            if (sortByPrice.HasValue && sortByPrice != 0)
            {
                products.OrderBy(p => p.Price);
            }

            if (sortByDate.HasValue && sortByDate != 0)
            {
                products.OrderBy(p => p.ImportDate);
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

        public async Task<ProductDto?> GetProductAsync(string productId)
        {
            var product = await (from p in _context.Products
                                 where p.ProductID == productId
                                 select p).FirstOrDefaultAsync();

            if (product == null)
            {
                return null;
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
    }
}
