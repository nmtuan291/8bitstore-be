using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _8bitstore_be.Models
{
    public class Wishlist
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public ICollection<string> ProductId { get; set; }

        [ForeignKey("UserId")]
        [Required]
        public User User { get; set; }
    }
}
