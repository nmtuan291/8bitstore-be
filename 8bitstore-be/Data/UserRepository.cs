using _8bitstore_be.DTO.User;
using _8bitstore_be.Exceptions;
using _8bitstore_be.Interfaces.Repositories;
using _8bitstore_be.Interfaces.Services;
using _8bitstore_be.Models;
using Microsoft.EntityFrameworkCore;

namespace _8bitstore_be.Data;

public class UserRepository: Repository<User>, IUserRepository
{
    public UserRepository(_8bitstoreContext context) : base(context) {}

    public async Task<User?> GetByIdAsync(string id)
    {
        return await _context.Users
            .Include(u => u.Addresses)
            .FirstOrDefaultAsync(u => u.Id == id);
    }
    
    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users
            .Include(u => u.Addresses)
            .FirstOrDefaultAsync(u => u.UserName == username);
    }

    public async Task UpdateAddressAsync(AddressDto addressDto)
    {
        var address = await _context.Addresses.FindAsync(addressDto.Id);
        if (address != null)
        {
            address.AddressDetail =  addressDto.AddressDetail;
            address.City = addressDto.City;
            address.District = addressDto.District;
            address.Ward = addressDto.Ward;
            address.IsDefault = addressDto.IsDefault;
            address.Recipent =  addressDto.Recipent;
            address.RecipentPhone = addressDto.RecipentPhone;
        }
        
        await _context.SaveChangesAsync();
    }

    public async Task InsertAddressAsync(AddressDto addressDto, string userId)
    {
        var address = new Address()
        {
            Id = Guid.NewGuid(),
            AddressDetail = addressDto.AddressDetail,
            City = addressDto.City,
            District = addressDto.District,
            Ward = addressDto.Ward,
            UserId = userId,
            Recipent = addressDto.Recipent,
            IsDefault = addressDto.IsDefault,
            RecipentPhone = addressDto.RecipentPhone,
        };
        
        await _context.Addresses.AddAsync(address);
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<Address>> GetAddressesByUserIdAsync(string userId)
    {
        return await _context.Addresses
            .Where(a => a.UserId == userId)
            .ToListAsync();
    }

    public async Task DeleteAddressById(Guid id)
    {
       int affected = await _context.Addresses
            .Where(a => a.Id == id)
            .ExecuteDeleteAsync();
        
        if (affected == 0)
            throw new AddressException("Address does not exist");
    }
}