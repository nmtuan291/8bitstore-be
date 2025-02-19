using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace _8bitstore_be.Services
{
    public class AuthService
    {
        private readonly string _userName;
        private readonly IConfiguration _config;

        public AuthService(string userName, IConfiguration config)
        {
            _config = config;
            _userName = userName;
        }

        public string GenerateAccessToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt: Access_Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim()
            };
        }

    }
}
