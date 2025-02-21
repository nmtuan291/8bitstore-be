using Microsoft.AspNetCore.Identity;
using _8bitstore_be.DTO;

namespace _8bitstore_be.Interfaces
{
    public interface IRegistrationService
    {
        public Task<AuthResponseDto> SignupAsync(UserForRegistrationDto userInfo);
    }
}
