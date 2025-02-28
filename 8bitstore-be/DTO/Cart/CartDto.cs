using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using _8bitstore_be.Models;

namespace _8bitstore_be.DTO.Cart
{
    public class CartDto
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public ICollection<CartItemDto> CartItems { get; set; }
    }
}
