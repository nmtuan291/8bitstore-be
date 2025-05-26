using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _8bitstore_be.Models
{
    public class WishlistItem
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string WishlistId { get; set; }

        [ForeignKey("WishlistId")]
        [Required]
        public Wishlist Wishlist { get; set; }

        [Required]
        public string ProductId { get; set; }

        [ForeignKey("ProductId")]
        [Required]
        public Product Product { get; set; }
    }
}
