using _8bitstore_be.DTO.Order;
using _8bitstore_be.Interfaces.Services;
using _8bitstore_be.Interfaces.Repositories;
using _8bitstore_be.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace _8bitstore_be.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IEmailService _emailService;
        private readonly UserManager<User> _userManager;

        public OrderService(IOrderRepository orderRepository, IEmailService emailService, UserManager<User> userManager)
        {
            _orderRepository = orderRepository;
            _emailService = emailService;
            _userManager = userManager;
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
            
            // Send email after creating order
            string subject = $"Xác nhận đơn hàng {orderId}";
            var user = await _userManager.FindByIdAsync(userId);
            string userEmail = user.Email;
            
            string emailBody = $@"
                <h2>Xác nhận đã đặt đơn hàng {orderId}</h2>
                <p>Cảm ơn vì đã đặt hành, {user.FullName}!</p>
                <p><strong>Ngày đặt:</strong> {newOrder.OrderDate}</p>
                <p><strong>Trạng thái:</strong> {newOrder.Status}</p>
                <h3>Danh sách hàng:</h3>
                <ul>
                    {string.Join("", newOrder.OrderProducts.Select(item => $"<li>{item.Product?.ProductName ?? item.ProductId} - {item.Quantity} x {item.UnitPrice:C}</li>"))}
                </ul>
                <p><strong>Tổng cộng:</strong> {newOrder.Total:C}</p>
            ";
            await _emailService.SendEmailAsync(userEmail, subject, emailBody);
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
