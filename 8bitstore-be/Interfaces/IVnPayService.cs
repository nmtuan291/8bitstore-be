using _8bitstore_be.DTO.Payment;

namespace _8bitstore_be.Interfaces
{
    public interface IVnPayService
    {
        public string CreatePaymentUrl(HttpContext context);
        public Task savePaymentAsync(VnPayResultDto result, string userId);
    }
}
