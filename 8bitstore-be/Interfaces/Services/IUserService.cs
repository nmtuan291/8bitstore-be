using System.Threading.Tasks;

namespace _8bitstore_be.Interfaces.Services
{
    public interface IUserService
    {
        Task ChangeAddressAsync(string userId, string address, string city, string district, string subDistrict);
        Task ChangePasswordAsync(string userId, string newPassword, string currentPassword);
        Task ForgotPasswordAsync(string userId);
    }
} 