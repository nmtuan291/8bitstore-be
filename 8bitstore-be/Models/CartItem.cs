using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _8bitstore_be.Models
{
    public class CartItem
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Required]
        public string ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [ForeignKey("CartId")]
        [Required]
        public Cart Cart { get; set; }

        [Required]
        public string CartId { get; set; }
    }
}
