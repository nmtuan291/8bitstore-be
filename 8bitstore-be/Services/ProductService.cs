using System.Linq;
using _8bitstore_be.Data;
using _8bitstore_be.Interfaces;
using _8bitstore_be.Models;
using Microsoft.EntityFrameworkCore;

namespace _8bitstore_be.Services
{
    public class ProductService
    {
        private readonly _8bitstoreContext _context;
        public ProductService(_8bitstoreContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var products = from product in _context.Products
                           select product;
            return await products.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetFilteredProducts(int priceSortOrder, int charSortOrder, int latestSortOrder)
        {
            var products = from product in _context.Products
                           select product;
            return await products.ToListAsync();
        }
    }
}
