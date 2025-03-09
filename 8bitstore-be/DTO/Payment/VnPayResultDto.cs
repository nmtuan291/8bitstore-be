using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.SignalR;

namespace _8bitstore_be.DTO.Payment
{
    public class VnPayResultDto
    {
        [Required]
        public string UserId { get; set; }

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
