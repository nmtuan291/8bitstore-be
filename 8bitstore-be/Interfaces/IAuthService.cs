using _8bitstore_be.DTO;

namespace _8bitstore_be.Interfaces
{
    public interface IAuthService
    {
        public string GenerateAccessToken(string userName);
        public string GenerateRefreshToken(string userName);
    }
}
