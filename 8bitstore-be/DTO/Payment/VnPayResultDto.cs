using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.SignalR;

namespace _8bitstore_be.DTO.Payment
{
    public class VnPayResultDto
    {
        [Required]
        public string TmnCode { get; set; }

        [Required]
        public string Amount {  get; set; }

        [Required]
        public string OrderInfo { get; set; }

        [Required]
        public string TransactionNo { get; set; }

        [Required]
        public string BankCode { get; set; }

        [Required]
        public string BankTranNo { get; set; }

        [Required]
        public string CardType { get; set; }

        [Required]
        public string PayDate { get; set; }

        [Required]
        public string ResponseCode { get; set; }

        [Required]
        public string SecureHash { get; set; }

        [Required]
        public string TxnRef { get; set; }

        [Required]
        public string TransactionStatus { get; set; }

        [Required]
        public string OrderId { get; set; }
    }
}
