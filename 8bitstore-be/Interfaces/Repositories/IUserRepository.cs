using _8bitstore_be.DTO.User;
using _8bitstore_be.Models;

namespace _8bitstore_be.Interfaces.Repositories;

public interface IUserRepository: IRepository<User>
{
    new Task<User?> GetByIdAsync(string id);
    Task<User?> GetByUsernameAsync(string username);
    Task UpdateAddressAsync(AddressDto addressDto);
    Task InsertAddressAsync(AddressDto addressDto, string userId);
    Task<List<Address>> GetAddressesByUserIdAsync(string userId);
    Task DeleteAddressById(Guid id);
}