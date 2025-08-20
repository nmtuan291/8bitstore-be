using _8bitstore_be.DTO.User;
using _8bitstore_be.Exceptions;
using _8bitstore_be.Interfaces.Services;
using _8bitstore_be.Models;
using Microsoft.AspNetCore.Identity;

namespace _8bitstore_be.Services
{
    public class RegistrationService: IRegistrationService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegistrationService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager) 
        { 
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<AuthResponseDto> SignupAsync(UserForRegistrationDto userInfo) 
        {
            User user = new User
            {
                FullName = userInfo.FullName,
                UserName = userInfo.UserName,
                Email = userInfo.Email,
                PhoneNumber = userInfo.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, userInfo.Password);

            if (result.Succeeded)
            {
                var existedRole = await _roleManager.RoleExistsAsync("User");

                if (!existedRole)
                {
                    var role = new IdentityRole("User");
                    await _roleManager.CreateAsync(role);
                }

                var addToRoleResult = await _userManager.AddToRoleAsync(user, "User");

                if (!addToRoleResult.Succeeded)
                {
                    return new AuthResponseDto
                    {
                        isSuccess = false,
                        Errors = addToRoleResult.Errors.Select(e => e.Description)
                    };
                }
            }

            return new AuthResponseDto
            {
                isSuccess = result.Succeeded,
                Errors = result.Errors.Select(e => e.Description)
            }; ;
        }

    }
}
