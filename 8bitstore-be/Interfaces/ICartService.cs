using _8bitstore_be.DTO.Cart;

namespace _8bitstore_be.Interfaces
{
    public interface ICartService
    {
        public Task<bool> AddItemAsync(string userId, string productId, int quantity);
        public Task<CartDto> GetCartAsync(string userId);
    }
}
