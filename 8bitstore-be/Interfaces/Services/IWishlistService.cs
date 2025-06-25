using _8bitstore_be.DTO.Wishlist;
using System.Threading.Tasks;

namespace _8bitstore_be.Interfaces.Services
{
    public interface IWishlistService
    {
        Task<WishlistDto> GetWishlistAsync(string userId);
        Task AddItemAsync(string productId, string userId);
        Task RemoveItemAsync(string userId, string productId);
    }
} 