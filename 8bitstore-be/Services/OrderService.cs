using _8bitstore_be.DTO.Order;
using _8bitstore_be.Interfaces.Services;
using _8bitstore_be.Interfaces.Repositories;
using _8bitstore_be.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _8bitstore_be.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task CreateOrderAsync(OrderDto order, string userId)
        {
            string orderId = order.OrderId;
            Order newOrder = new()
            {
                Id = orderId,
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                DeliveryDate = null,
                Status = order.Status,
                Total = order.Total ?? 0,
                OrderProducts = order.Items.Select(item => new OrderProduct
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    OrderId = orderId,
                    UnitPrice = item.Price
                }).ToList()
            };
            await _orderRepository.AddAsync(newOrder);
            await _orderRepository.SaveChangesAsync();
        }

        public async Task<ICollection<OrderDto>> GetOrderAsync(string userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            return orders.Select(x => new OrderDto
            {
                Items = x.OrderProducts.Select(p => new OrderItemDto
                {
                    ProductId = p.ProductId,
                    Quantity = p.Quantity,
                    ProductName = p.Product?.ProductName,
                    Price = p.UnitPrice,
                    ImgUrl = p.Product?.ImgUrl
                }).ToList(),
                Total = x.Total,
                Status = x.Status,
                OrderDate = x.OrderDate,
                DeliveryDate = x.DeliveryDate,
                OrderId = x.Id,
            }).OrderBy(x => x.OrderDate).ToList();
        }

        public async Task ChangeOrderStatusAsync(OrderDto request)
        {
            var orders = await _orderRepository.FindAsync(o => o.Id == request.OrderId);
            var order = orders.FirstOrDefault();
            if (order != null && order.Status != request.Status)
            {
                order.Status = request.Status;
                await _orderRepository.SaveChangesAsync();
            }
        }
    }
}
