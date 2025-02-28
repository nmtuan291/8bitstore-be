using System.ComponentModel.DataAnnotations;

namespace _8bitstore_be.DTO.Cart
{
    public class CartItemDto
    {

        [Required]
        public string ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public string ImgUrl { get; set; }
    }
}
