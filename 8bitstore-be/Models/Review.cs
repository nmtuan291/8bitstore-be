using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _8bitstore_be.Models
{
    public class Review
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Required]
        public int Score { get; set; }

        [Required]
        public string Comment { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public DateTime ReviewDate { get; set; }
    }
}
