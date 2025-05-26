using _8bitstore_be.Data;
using _8bitstore_be.Interfaces;
using _8bitstore_be.Models;
using Microsoft.AspNetCore.Identity;

namespace _8bitstore_be.Services
{
    public class UserService: IUserService
    {
        private readonly _8bitstoreContext _context;
        private readonly UserManager<User> _userManager;

        public UserService(_8bitstoreContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task ChangeAddressAsync(string userId, string address, string city, string district, string subDistrict)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException("User does not exists");
            }

            user.Address = address;
            user.City = city;
            user.District = district;
            user.SubDistrict = subDistrict;

            await _context.SaveChangesAsync();
        }

        public async Task ChangePasswordAsync(string userId, string newPassword, string currentPassword)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User does not exists");
            }

            await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        public async Task ForgotPasswordAsync(string userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return;
            }

            var token = _userManager.GeneratePasswordResetTokenAsync(user);

        }
    }
}
