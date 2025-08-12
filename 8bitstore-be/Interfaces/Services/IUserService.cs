using System.Threading.Tasks;
using _8bitstore_be.DTO.User;

namespace _8bitstore_be.Interfaces.Services
{
    public interface IUserService
    {
        Task<bool> ChangeAddressAsync(AddressDto addressDto, string userId);
        Task ChangePasswordAsync(string userId, string newPassword, string currentPassword);
        Task ForgotPasswordAsync(string userId);
        Task<bool> AddAddressAsync(AddressDto addressDto, string userId);
        Task<List<AddressDto>> GetAddressesByUserIdAsync(string userId);
        Task<bool> DeleteAddressAsync(Guid id);
        Task<List<string>> GetUserRoleAsync(string userId);
    }
} 