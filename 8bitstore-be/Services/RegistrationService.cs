using _8bitstore_be.DTO;
using _8bitstore_be.Interfaces;
using _8bitstore_be.Models;
using Microsoft.AspNetCore.Identity;

namespace _8bitstore_be.Services
{
    public class RegistrationService: IRegistrationService
    {
        private readonly UserManager<User> _userManager;

        public RegistrationService(UserManager<User> userManager) 
        { 
            _userManager = userManager;
        }
        public async Task<RegistrationResponseDto> SignupAsync(UserForRegistrationDto userInfo) 
        {
            User user = new User
            {
                FullName = userInfo.FullName,
                Id = userInfo.Email,
                Address = userInfo.Address,
                UserName = userInfo.UserName,
                Email = userInfo.Email
            };

            var result = await _userManager.CreateAsync(user, userInfo.Password);

            RegistrationResponseDto response = new RegistrationResponseDto
            {
                isSuccess = result.Succeeded,
                Errors = result.Errors.Select(e => e.Description)
            };

            return response;
        }

    }
}
