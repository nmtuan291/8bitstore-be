using _8bitstore_be.Interfaces.Repositories;
using _8bitstore_be.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _8bitstore_be.Data
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(_8bitstoreContext context) : base(context) { }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            return await _context.Products.Where(p => p.ProductName.Contains(name)).ToListAsync();
        }

        /*
        public async Task<IEnumerable<Product>> GetProductsAsAdminAsync()
        {
            return await _context.Products.ToListAsync();
        }*/
    }
} 