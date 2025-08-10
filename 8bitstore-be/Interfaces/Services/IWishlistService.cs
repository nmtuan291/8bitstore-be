using _8bitstore_be.DTO.Wishlist;
using System.Threading.Tasks;

namespace _8bitstore_be.Interfaces.Services
{
    public interface IWishlistService
    {
        Task<WishlistDto> GetWishlistAsync(string userId);
        Task<bool> AddItemAsync(string productId, string userId);
        Task<bool> RemoveItemAsync(string userId, string productId);
    }
} 