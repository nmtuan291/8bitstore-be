using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using _8bitstore_be.DTO.User;

namespace _8bitstore_be.DTO.Order
{
    public class OrderDto
    {
        public OrderDto()
        {
            Items = new List<OrderItemDto>();  // Initialize the collection
        }
        public required string OrderId { get; set; }
        public List<OrderItemDto>? Items { get; set; }
        public decimal? Total { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public required string Status { get; set; }
        public string? user { get; set; }
        public string? phone { get; set; }
        public Guid? AddressId { get; set; }
        public AddressDto? Address { get; set; }
    }
}
