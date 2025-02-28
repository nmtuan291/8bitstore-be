using _8bitstore_be.DTO;
using _8bitstore_be.Models;

namespace _8bitstore_be.Interfaces
{
    public interface IAuthService
    {
        public Task<string> GenerateAccessToken(User user);
        public string GenerateRefreshToken();
    }
}
