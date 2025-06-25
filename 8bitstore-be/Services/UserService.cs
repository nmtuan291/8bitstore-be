using _8bitstore_be.Interfaces;
using _8bitstore_be.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _8bitstore_be.Interfaces.Repositories;
using _8bitstore_be.Interfaces.Services;

namespace _8bitstore_be.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly UserManager<User> _userManager;

        public UserService(IRepository<User> userRepository, UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public async Task ChangeAddressAsync(string userId, string address, string city, string district, string subDistrict)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User does not exists");
            user.Address = address;
            user.City = city;
            user.District = district;
            user.SubDistrict = subDistrict;
            await _userRepository.SaveChangesAsync();
        }

        public async Task ChangePasswordAsync(string userId, string newPassword, string currentPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User does not exists");
            await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        public async Task ForgotPasswordAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return;
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            // You may want to send this token via email or other means
        }
    }
}
