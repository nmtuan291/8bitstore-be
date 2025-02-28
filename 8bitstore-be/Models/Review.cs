using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _8bitstore_be.Models
{
    public class Review
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Required]
        public string ProductId { get; set; }

        public int Score { get; set; }
        public string Comment { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
