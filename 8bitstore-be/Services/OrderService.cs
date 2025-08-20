using _8bitstore_be.DTO.Order;
using _8bitstore_be.Interfaces.Services;
using _8bitstore_be.Interfaces.Repositories;
using _8bitstore_be.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _8bitstore_be.DTO.User;
using _8bitstore_be.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace _8bitstore_be.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IEmailService _emailService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<OrderService> _logger;
        public OrderService(IOrderRepository orderRepository, IEmailService emailService, 
            UserManager<User> userManager, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _emailService = emailService;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task CreateOrderAsync(OrderDto order, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                    throw new UserNotFoundException(userId);
                
            if (string.IsNullOrEmpty(order.OrderId))
                throw new OrderNotFoundException(order.OrderId);
            
            string orderId = order.OrderId;
            Order newOrder = new()
            {
                Id = orderId,
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                DeliveryDate = null,
                Status = order.Status,
                Total = order.Total ?? 0,
                AddressId = order.AddressId ?? Guid.Empty,
                OrderProducts = (order.Items ?? new List<OrderItemDto>()).Select(item => new OrderProduct
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
            
            string subject = $"Xác nhận đơn hàng {orderId}";
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw  new UserNotFoundException(userId);
            
            string userEmail = user.Email ?? "";
            
            string emailBody = $@"
                <h2>Xác nhận đã đặt đơn hàng {orderId}</h2>
                <p>Cảm ơn vì đã đặt hàng, {user.FullName}!</p>
                <p><strong>Ngày đặt:</strong> {newOrder.OrderDate}</p>
                <p><strong>Trạng thái:</strong> {newOrder.Status}</p>
                <h3>Danh sách hàng:</h3>
                <ul>
                    {string.Join("", newOrder.OrderProducts.Select(item => $"<li>{item.Product?.ProductName ?? item.ProductId} - {item.Quantity} x {item.UnitPrice:C}</li>"))}
                </ul>
                <p><strong>Tổng cộng:</strong> {newOrder.Total:C}</p>
            ";
            await _emailService.SendEmailAsync(userEmail, emailBody, subject);
        }

        public async Task<List<OrderDto>> GetOrderAsync(string userId)
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
                    ImgUrl = p.Product?.ImgUrl?.ToList()
                }).ToList(),
                Total = x.Total,
                Status = x.Status,
                OrderDate = x.OrderDate,
                DeliveryDate = x.DeliveryDate,
                OrderId = x.Id,
                AddressId = x.AddressId,
                Address = new AddressDto
                {
                    Id = x.AddressId,
                    AddressDetail = x.Address?.AddressDetail,
                    City = x.Address?.City,
                    District = x.Address?.District,
                    Ward = x.Address?.Ward,
                    IsDefault = x.Address.IsDefault,
                    Recipent = x.Address.Recipent,
                    RecipentPhone = x.Address.RecipentPhone,
                }
            }).OrderBy(x => x.OrderDate).ToList();
        }

        public async Task<List<OrderDto>> GetOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return orders.Select(x => new OrderDto
            {
                Items = x.OrderProducts.Select(p => new OrderItemDto
                {
                    ProductId = p.ProductId,
                    Quantity = p.Quantity,
                    ProductName = p.Product?.ProductName,
                    Price = p.UnitPrice,
                    ImgUrl = p.Product?.ImgUrl?.ToList()
                }).ToList(),
                Total = x.Total,
                Status = x.Status,
                OrderDate = x.OrderDate,
                DeliveryDate = x.DeliveryDate,
                OrderId = x.Id,
                user = x.User.FullName,
                phone = x.User.PhoneNumber,
                AddressId = x.AddressId,
                Address = new AddressDto
                {
                    Id = x.AddressId,
                    AddressDetail = x.Address?.AddressDetail,
                    City = x.Address?.City,
                    District = x.Address?.District,
                    Ward = x.Address?.Ward,
                    IsDefault = x.Address.IsDefault,
                    Recipent = x.Address.Recipent,
                    RecipentPhone = x.Address.RecipentPhone,
                }
            }).OrderBy(x => x.OrderDate).ToList();
        }
        
        public async Task ChangeOrderStatusAsync(OrderDto request)
        {
            if (string.IsNullOrEmpty(request.OrderId))
                throw  new OrderNotFoundException(request.OrderId);

            var orders = await _orderRepository.FindAsync(o => o.Id == request.OrderId);
            var order = orders.FirstOrDefault();

            if (order != null && order.Status != request.Status)
            {
                if (order.Status == "cancelled" || order.Status == "delivered")
                {
                    _logger.LogWarning($"Order {request.OrderId} status cannot be changed");
                    throw new OrderCompletedException(request.OrderId);
                }
                order.Status = request.Status;
                await _orderRepository.SaveChangesAsync();
            }
        }
    }
}
