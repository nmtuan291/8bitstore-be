using _8bitstore_be.Models;

namespace _8bitstore_be.Interfaces
{
    public interface IProductService
    {
        public Task<IEnumerable<Product>> GetProductsAsync();
        public Task<IEnumerable<Product>> GetFilteredProductsAsync();
    }
}
