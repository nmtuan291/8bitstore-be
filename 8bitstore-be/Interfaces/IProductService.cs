using _8bitstore_be.Models;
using _8bitstore_be.DTO;

namespace _8bitstore_be.Interfaces
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductDto>> GetProductsAsync(int? sortByName, int? sortByPrice, int? sortByDate);
        public Task<ProductDto?> GetProductAsync(string productId);
    }
}
