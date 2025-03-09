using System.ComponentModel.DataAnnotations;

namespace _8bitstore_be.Models
{
    public class PaymentVnPay: Payment
    {
        [Required]
        public string TransactionNo { get; set; }

        [Required]
        public string BankCode { get; set; }

        [Required]
        public string BankTranNo { get; set; }

        [Required]
        public string CardType { get; set; }

        [Required]
        public DateTime PayDate { get; set; }

        [Required]
        public string ResponseCode { get; set; }
    }
}
