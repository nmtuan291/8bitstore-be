using _8bitstore_be.DTO.User;
using System.Threading.Tasks;

namespace _8bitstore_be.Interfaces.Services
{
    public interface ILoginService
    {
        Task<AuthResponseDto> LoginAsync(UserLoginDto user);
        Task<UserDto?> GetUserAsync(string userId);
        Task LogoutAsync();
    }
} 