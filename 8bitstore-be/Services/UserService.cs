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

        public async Task ChangeAddressAsync(AddressDto addressDto)
        {
            await _userRepository.UpdateAddressAsync(addressDto);
        }

        public async Task AddAddressAsync(AddressDto addressDto, string userId)
        {
            await _userRepository.InsertAddressAsync(addressDto, userId);
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
            
            // Not done
        }

        public async Task<IEnumerable<AddressDto>> GetAddressesByUserIdAsync(string userId)
        {
            var addresses = await _userRepository.GetAddressesByUserIdAsync(userId);

            return addresses.Select(address => new AddressDto()
            {
                Id = address.Id,
                AddressDetail = address.AddressDetail,
                City = address.City,
                District = address.District,
                Ward = address.Ward,
                Recipent = address.Recipent,
                RecipentPhone = address.RecipentPhone,
                IsDefault = address.IsDefault
            });
        }

        public async Task DeleteAddressAsync(Guid id)
        {
            await _userRepository.DeleteAddressById(id);
        }
    }
}
