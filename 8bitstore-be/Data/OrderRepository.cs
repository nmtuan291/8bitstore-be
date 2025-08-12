using _8bitstore_be.Interfaces.Repositories;
using _8bitstore_be.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _8bitstore_be.Data
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(_8bitstoreContext context) : base(context) { }

        public async Task<List<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .Include(o => o.Address)
                .ToListAsync();
        }
        
        public override async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .Include(o => o.User)
                .Include(o => o.Address)
                .ToListAsync();
        }

        public override async Task AddAsync(Order entity)
        {
            foreach (var item in entity.OrderProducts)
            {
                var product = await _context.Products.SingleOrDefaultAsync(p => p.ProductID == item.ProductId);
                if (product != null)
                {
                    product.StockNum -=  item.Quantity;
                    product.WeeklySales += item.Quantity;
                }
            }
            await base.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
    }
} 