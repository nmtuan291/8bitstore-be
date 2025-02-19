using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _8bitstore_be.Models
{
    public class OrderProduct
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey("OrderId")]
        [Required]
        public Order Order { get; set; }

        public string OrderId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public string ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

    }
}
