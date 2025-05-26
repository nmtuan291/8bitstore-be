using System.ComponentModel.DataAnnotations;

namespace _8bitstore_be.DTO.Payment
{
    public class PaymentRequest
    {
        [Required]
        public string Amount { get; set; }
    }
}
