using _8bitstore_be.Models;
using System.Threading.Tasks;

namespace _8bitstore_be.Interfaces.Repositories
{
    public interface IWishlistRepository : IRepository<Wishlist>
    {
        Task<Wishlist> GetWishlistByUserIdAsync(string userId);
    }
} 