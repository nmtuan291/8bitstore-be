using _8bitstore_be.DTO;
using _8bitstore_be.DTO.Payment;

namespace _8bitstore_be.Interfaces
{
    public interface IVnPayService
    {
        public string CreatePaymentUrl(HttpContext context, string amount);
        public Task<StatusResponse<string>> savePaymentAsync(VnPayResultDto result, string userId);
    }
}
