using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _8bitstore_be.Models
{
    public class CartItem
    {
        [Key]
        public string Id { get; set; }
        public string ProductId { get; set; }

        [ForeignKey("ProductId")]
        [Required]
        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
