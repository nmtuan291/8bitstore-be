using _8bitstore_be.DTO.Order;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _8bitstore_be.Interfaces.Services
{
    public interface IOrderService
    {
        Task<bool> CreateOrderAsync(OrderDto order, string userId);
        Task<List<OrderDto>> GetOrderAsync(string userId);
        Task<bool> ChangeOrderStatusAsync(OrderDto request);
        Task<List<OrderDto>> GetOrdersAsync();
    }
} 