using _8bitstore_be.Interfaces.Repositories;
using _8bitstore_be.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace _8bitstore_be.Data
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(_8bitstoreContext context) : base(context) { }

        public async Task<Cart?> GetCartByUserIdAsync(string userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .SingleOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task EmptyCartAsync(string userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .SingleOrDefaultAsync(c => c.UserId == userId);

            if (cart != null)
            {
                _context.CartItems.RemoveRange(cart.CartItems);
            }
            await _context.SaveChangesAsync();
        } 
    }
} 