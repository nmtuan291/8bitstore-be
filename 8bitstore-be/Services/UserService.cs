using _8bitstore_be.Interfaces;
using _8bitstore_be.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _8bitstore_be.DTO.User;
using _8bitstore_be.Exceptions;
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

        public async Task ChangeAddressAsync(AddressDto addressDto, string userId)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                throw new UserNotFoundException(userId);
            
            if (addressDto.Id == Guid.Empty)
                throw new AddressException("Address cannot be empty");

            if (addressDto.IsDefault)
            {
                var addresses = await GetAddressesByUserIdAsync(userId);
                foreach (var address in addresses)
                {
                    if (address.Id != addressDto.Id)
                    {
                        var newAddress = new AddressDto
                        {
                            Id = address.Id,
                            IsDefault = false,
                            Recipent = address.Recipent,
                            City = address.City,
                            District = address.District,
                            Ward = address.Ward,
                            RecipentPhone = address.RecipentPhone,
                            AddressDetail = address.AddressDetail,
                        };
                        await _userRepository.UpdateAddressAsync(newAddress);
                    }
                }
            }
                
            await _userRepository.UpdateAddressAsync(addressDto);
            await _userRepository.SaveChangesAsync();
        }

        public async Task<List<string>> GetUserRoleAsync(string userId)
        {
            var user  = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new List<string>();
            
            return (await _userManager.GetRolesAsync(user)).ToList();
        }

        public async Task AddAddressAsync(AddressDto addressDto, string userId)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                throw new UserNotFoundException(userId);
            
            await _userRepository.InsertAddressAsync(addressDto, userId);
            await _userRepository.SaveChangesAsync();
        }

        public async Task ChangePasswordAsync(string userId, string newPassword, string currentPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new UserNotFoundException(userId);
            await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        public async Task ForgotPasswordAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User does not exists");
            string code = await _userManager.GeneratePasswordResetTokenAsync(user);
            // TODO
        }

        public async Task<List<AddressDto>> GetAddressesByUserIdAsync(string userId)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                throw new  UserNotFoundException(userId);
            
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
            }).ToList();
        }

        public async Task DeleteAddressAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new AddressException("Address cannot be empty");

            await _userRepository.DeleteAddressById(id);
            await _userRepository.SaveChangesAsync();
        }
    }
}
