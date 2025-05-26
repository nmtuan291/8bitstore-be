using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _8bitstore_be.Models
{
    public class Wishlist
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public ICollection<WishlistItem> Products { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public string UserId { get; set; }

    }
}
