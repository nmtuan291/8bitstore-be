using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using _8bitstore_be.DTO;
using _8bitstore_be.Models;
using Microsoft.IdentityModel.Tokens;
using _8bitstore_be.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace _8bitstore_be.Services
{
    public class AuthService: IAuthService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;

        public AuthService(IConfiguration config, UserManager<User> userManager)
        {
            _config = config;
            _userManager = userManager;
        }

        public async Task<string> GenerateAccessToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Access_Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            if (roles != null)
            {
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            }

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            byte[] randomBytes = new byte[64];
            RandomNumberGenerator.Fill(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }
    }
}
