using _8bitstore_be.Models;
using _8bitstore_be.DTO.Product;

namespace _8bitstore_be.Interfaces
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductDto>> GetProductsAsync(ProductRequest request);
        public Task<ProductDto?> GetProductAsync(string productId);
        public Task AddProductAsync(ProductDto product);
        public Task<IEnumerable<string>> GetSuggestionAsync(string query);
    }
}
