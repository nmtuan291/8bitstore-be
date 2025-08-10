using _8bitstore_be.Interfaces;
using _8bitstore_be.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _8bitstore_be.DTO.User;
using _8bitstore_be.Interfaces.Repositories;
using _8bitstore_be.Interfaces.Services;

namespace _8bitstore_be.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;

        public UserService(IUserRepository userRepository, UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public async Task<bool> ChangeAddressAsync(AddressDto addressDto)
        {
            try
            {
                if (addressDto == null || addressDto.Id == Guid.Empty)
                    return false;

                await _userRepository.UpdateAddressAsync(addressDto);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<string>> GetUserRoleAsync(string userId)
        {
            var user  = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new List<string>();
            
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> AddAddressAsync(AddressDto addressDto, string userId)
        {
            try
            {
                if (addressDto == null || string.IsNullOrEmpty(userId))
                    return false;

                await _userRepository.InsertAddressAsync(addressDto, userId);
                return true;
            }
            catch
            {
                return false;
            }
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
                throw new KeyNotFoundException("User does not exists");
            string code = await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IEnumerable<AddressDto>> GetAddressesByUserIdAsync(string userId)
        {
            var addresses = await _userRepository.GetAddressesByUserIdAsync(userId);
            return addresses.Select(a => new AddressDto
            {
                Id = a.Id,
                AddressDetail = a.AddressDetail,
                City = a.City,
                District = a.District,
                Ward = a.Ward,
                IsDefault = a.IsDefault,
                Recipent = a.Recipent,
                RecipentPhone = a.RecipentPhone
            });
        }

        public async Task<bool> DeleteAddressAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return false;

                await _userRepository.DeleteAddressById(id);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
