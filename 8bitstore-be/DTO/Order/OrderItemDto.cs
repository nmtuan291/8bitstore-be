using System.ComponentModel.DataAnnotations;

namespace _8bitstore_be.DTO.Order
{
    public class OrderItemDto
    {
        [Required]
        public string ProductId { get; set; }

        public string? ProductName { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        public ICollection<string>? ImgUrl { get; set; }

    }
}
