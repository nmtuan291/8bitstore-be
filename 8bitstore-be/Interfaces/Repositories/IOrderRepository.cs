using _8bitstore_be.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _8bitstore_be.Interfaces.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
        Task<IEnumerable<Order>> GetAllAsync();
    }
} 