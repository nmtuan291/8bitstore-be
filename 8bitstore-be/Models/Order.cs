using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _8bitstore_be.Models
{
    public class Order
    {
        [Key]
        public string Id { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        [Required]
        public User User { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public decimal Total {  get; set; }
        
        [Required]
        public Guid AddressId { get; set; }
        
        [ForeignKey("AddressId")]
        public Address Address { get; set; }

        public ICollection<OrderProduct> OrderProducts { get; set; }

    }
}
