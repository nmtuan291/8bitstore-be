using _8bitstore_be.Data;
using _8bitstore_be.DTO.Order;
using _8bitstore_be.Interfaces;
using _8bitstore_be.Models;
using Microsoft.EntityFrameworkCore;

namespace _8bitstore_be.Services
{
    public class OrderService: IOrderService
    {
        private readonly _8bitstoreContext _context;

        public OrderService(_8bitstoreContext context)
        {
            _context = context;
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
                OrderProducts = new List<OrderProduct>()
            };

            foreach (var item in order.Items)
            {
                OrderProduct newProduct = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    OrderId = orderId,
                    UnitPrice = item.Price
                };

                newOrder.OrderProducts.Add(newProduct); // Add to navigation property
                await _context.OrderItems.AddAsync(newProduct); // You can keep using OrderItems here
            }

            await _context.AddAsync(newOrder);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<OrderDto>> GetOrderAsync(string userId)
        {
            var orders = await _context.Orders
                .Where(x => x.UserId == userId)
                .Include(x => x.OrderProducts)
                    .ThenInclude(p => p.Product)
                .Select(x => new OrderDto()
                {
                    Items = x.OrderProducts
                        .Select(p => new OrderItemDto
                        {
                            ProductId = p.ProductId,
                            Quantity = p.Quantity,
                            ProductName = p.Product.ProductName,
                            Price = p.UnitPrice,
                            ImgUrl = p.Product.ImgUrl
                        })
                        .ToList(),
                    Total = x.Total,
                    Status = x.Status,
                    OrderDate = x.OrderDate,
                    DeliveryDate = x.DeliveryDate,
                    OrderId = x.Id,
                })
                .OrderBy(x => x.OrderDate)
                .ToListAsync();

            return orders;
        }

        public async Task ChangeOrderStatusAsync(OrderDto request)
        {
            var order = await _context.Orders
                .Where(o => o.Id == request.OrderId)
                .FirstOrDefaultAsync();

            if (order != null && order.Status != request.Status)
            {
                order.Status = request.Status;
                await _context.SaveChangesAsync();
            }
        }
    }
}
