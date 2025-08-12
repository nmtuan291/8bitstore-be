using System.ComponentModel.DataAnnotations;

namespace _8bitstore_be.DTO.Wishlist
{
    public class WishlistItemDto
    {
        [Required]
        public string ProductId { get; set; }
        
        [Required]
        public string ProductName { get; set; }
        
        [Required]
        public decimal Price { get; set; }
        
        public List<string> ImgUrl { get; set; }
    }
}
