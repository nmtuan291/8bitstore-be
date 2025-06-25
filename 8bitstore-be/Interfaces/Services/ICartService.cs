using _8bitstore_be.DTO.Cart;
using System.Threading.Tasks;

namespace _8bitstore_be.Interfaces.Services
{
    public interface ICartService
    {
        Task AddItemAsync(string userId, string productId, int quantity);
        Task<CartDto> GetCartAsync(string userId);
        Task DeleteItemAsync(string userId, string productId);
    }
} 