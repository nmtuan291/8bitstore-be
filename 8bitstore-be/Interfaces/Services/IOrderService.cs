using _8bitstore_be.DTO.Order;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _8bitstore_be.Interfaces.Services
{
    public interface IOrderService
    {
        Task CreateOrderAsync(OrderDto order, string userId);
        Task<ICollection<OrderDto>> GetOrderAsync(string userId);
        Task ChangeOrderStatusAsync(OrderDto request);
        Task<ICollection<OrderDto>> GetOrdersAsync();
    }
} 