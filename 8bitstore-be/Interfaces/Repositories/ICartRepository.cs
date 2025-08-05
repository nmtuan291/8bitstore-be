using _8bitstore_be.Models;
using System.Threading.Tasks;

namespace _8bitstore_be.Interfaces.Repositories
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart?> GetCartByUserIdAsync(string userId);
        Task EmptyCartAsync(string userId);
    }
} 