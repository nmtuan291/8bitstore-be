using System.ComponentModel.DataAnnotations;

namespace _8bitstore_be.DTO.Cart
{
    public class AddItemRequestDto
    {
        [Required]
        public string ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
    