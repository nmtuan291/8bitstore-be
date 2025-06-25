using _8bitstore_be.DTO;
using _8bitstore_be.Models;
using System.Threading.Tasks;

namespace _8bitstore_be.Interfaces.Services
{
    public interface IAuthService
    {
        Task<string> GenerateAccessToken(User user);
        string GenerateRefreshToken();
    }
} 