using _8bitstore_be.DTO.User;
using System.Threading.Tasks;

namespace _8bitstore_be.Interfaces.Services
{
    public interface IRegistrationService
    {
        Task<AuthResponseDto> SignupAsync(UserForRegistrationDto userInfo);
    }
} 