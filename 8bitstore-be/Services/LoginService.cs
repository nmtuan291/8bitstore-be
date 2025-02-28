using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using _8bitstore_be.Data;
using _8bitstore_be.Models;
using System.IdentityModel.Tokens.Jwt;
using _8bitstore_be.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using _8bitstore_be.DTO.User;

namespace _8bitstore_be.Services
{
    public class LoginService : ILoginService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly _8bitstoreContext _context;

        public LoginService(SignInManager<User> signInManager, _8bitstoreContext context)
        {
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<AuthResponseDto> LoginAsync(UserLoginDto user)
        {
            var findUser = await (from u in _context.Users
                                  where u.UserName == user.UserName
                                  select u).FirstOrDefaultAsync();
            if (findUser == null)
            {
                return new AuthResponseDto
                {
                    isSuccess = false,
                    Errors = new List<string> { "User does not exist" }
                };
            }

            var result = await _signInManager.PasswordSignInAsync(findUser, user.Password, isPersistent: true, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                var errors = new List<string>();
                if (result.IsNotAllowed)
                {
                    errors.Add("Cannot sign in");
                }

                return new AuthResponseDto
                {
                    isSuccess = false,
                    Errors = errors
                };
            }

            /*string accessToken = await _authService.GenerateAccessToken(findUser);
            string refreshToken = _authService.GenerateRefreshToken();*/

            return new AuthResponseDto
            {
                isSuccess = true,
                User = new UserDto
                {
                    UserName = findUser.UserName,
                    Email = findUser.Email,
                    Address = findUser.Address,
                    FullName = findUser.FullName
                }
            };
        }

        public async Task<UserDto?> GetUserAsync(string userId)
        {
           return await _context.Users
                .Where(user => user.Id == userId)
                .Select(user => new UserDto
                {
                    UserName = user.UserName,
                    Address = user.Address,
                    FullName = user.FullName,
                    Email = user.Email
                })
                .SingleOrDefaultAsync();
        }
    }
}