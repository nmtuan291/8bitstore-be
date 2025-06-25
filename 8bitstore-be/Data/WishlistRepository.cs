using _8bitstore_be.Interfaces.Repositories;
using _8bitstore_be.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace _8bitstore_be.Data
{
    public class WishlistRepository : Repository<Wishlist>, IWishlistRepository
    {
        public WishlistRepository(_8bitstoreContext context) : base(context) { }

        public async Task<Wishlist> GetWishlistByUserIdAsync(string userId)
        {
            return await _context.Wishlists
                .Where(w => w.UserId == userId)
                .Include(w => w.Products)
                    .ThenInclude(wi => wi.Product)
                .SingleOrDefaultAsync();
        }
    }
} 