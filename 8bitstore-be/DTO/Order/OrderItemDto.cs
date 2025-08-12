using System.ComponentModel.DataAnnotations;

namespace _8bitstore_be.DTO.Order
{
    public class OrderItemDto
    {
        public required string ProductId { get; set; }

        public string? ProductName { get; set; }

        public required int Quantity { get; set; }

        public required decimal Price { get; set; }

        public List<string>? ImgUrl { get; set; }
    }
}
