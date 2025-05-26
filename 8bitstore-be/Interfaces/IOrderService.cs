using _8bitstore_be.DTO.Order;

namespace _8bitstore_be.Interfaces
{
    public interface IOrderService
    {
        public Task CreateOrderAsync(OrderDto order, string userId);
        public Task<ICollection<OrderDto>> GetOrderAsync(string userId);
        public Task ChangeOrderStatusAsync(OrderDto request);
    }
}
