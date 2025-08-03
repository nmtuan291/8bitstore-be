using _8bitstore_be.DTO.Product;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _8bitstore_be.Interfaces.Services
{
    public interface IProductService
    {
        Task<PaginatedResult> GetProductsAsync(ProductRequest request);
        Task<ProductDto?> GetProductAsync(string productId);
        Task AddProductAsync(ProductDto product);
        Task<IEnumerable<string>> GetSuggestionAsync(string query);
        Task<IEnumerable<ProductDto>> GetAllProductAsync();
    }
} 