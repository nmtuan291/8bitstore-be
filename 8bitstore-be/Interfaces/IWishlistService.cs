using _8bitstore_be.DTO.Wishlist;

namespace _8bitstore_be.Interfaces
{
    public interface IWishlistService
    {
        public Task<WishlistDto> GetWishlistAsync(string userId);
        public Task AddItemAsync(string productId, string userId);
        public Task RemoveItemAsync(string userId, string productId);
    }
}
