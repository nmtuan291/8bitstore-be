using Microsoft.AspNetCore.Identity;
using _8bitstore_be.DTO.User;

namespace _8bitstore_be.Interfaces
{
    public interface IRegistrationService
    {
        public Task<AuthResponseDto> SignupAsync(UserForRegistrationDto userInfo);
    }
}
