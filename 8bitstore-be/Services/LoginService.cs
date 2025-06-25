using Microsoft.AspNetCore.Identity;
using _8bitstore_be.Models;
using _8bitstore_be.Interfaces.Services;
using _8bitstore_be.Interfaces.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using _8bitstore_be.DTO.User;

namespace _8bitstore_be.Services
{
    public class LoginService : ILoginService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IRepository<User> _userRepository;

        public LoginService(SignInManager<User> signInManager, IRepository<User> userRepository)
        {
            _signInManager = signInManager;
            _userRepository = userRepository;
        }

        public async Task<AuthResponseDto> LoginAsync(UserLoginDto user)
        {
            var findUser = (await _userRepository.FindAsync(u => u.UserName == user.UserName)).FirstOrDefault();
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
            return new AuthResponseDto
            {
                isSuccess = true,
                User = new UserDto
                {
                    UserName = findUser.UserName,
                    Email = findUser.Email,
                    Address = findUser.Address,
                    FullName = findUser.FullName,
                    City = findUser.City,
                    District = findUser.District,
                    PhoneNumber = findUser.PhoneNumber
                }
            };
        }

        public async Task<UserDto?> GetUserAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;
            return new UserDto
            {
                UserName = user.UserName,
                Address = user.Address,
                FullName = user.FullName,
                Email = user.Email
            };
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}