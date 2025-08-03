using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace _8bitstore_be.DTO.Order
{
    public class OrderDto
    {
        public OrderDto()
        {
            Items = new List<OrderItemDto>();  // Initialize the collection
        }

        [Required]
        public string OrderId { get; set; }

        public ICollection<OrderItemDto>? Items { get; set; }

        public decimal? Total { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        [Required]
        public string Status { get; set; }

        public string? user { get; set; }
        public string? phone { get; set; }
    }
}
