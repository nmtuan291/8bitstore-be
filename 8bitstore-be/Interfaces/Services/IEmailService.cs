using System.Threading.Tasks;

namespace _8bitstore_be.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string body, string subject);
    }
} 