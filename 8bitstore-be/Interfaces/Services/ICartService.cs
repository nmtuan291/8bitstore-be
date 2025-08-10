using _8bitstore_be.DTO.Cart;
using System.Threading.Tasks;

namespace _8bitstore_be.Interfaces.Services
{
    public interface ICartService
    {
        Task<bool> AddItemAsync(string userId, string productId, int quantity);
        Task<CartDto> GetCartAsync(string userId);
        Task<bool> EmptyCartAsync(string userId);
        Task<bool> DeleteItemAsync(string userId, string productId);
    }
} 