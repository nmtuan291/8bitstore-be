using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _8bitstore_be.Models
{
    public class OrderProduct
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        [Required]
        public string OrderId { get; set; }

        public Product Product { get; set; }
        public string ProductId { get; set; }


        [Required]
        public int Quantity { get; set; }

    }
}
