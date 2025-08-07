using _8bitstore_be.DTO;
using _8bitstore_be.DTO.Payment;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace _8bitstore_be.Interfaces.Services
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, string amount);
        Task<StatusResponse<string>> SavePaymentAsync(VnPayResultDto result, string userId);
    }
} 