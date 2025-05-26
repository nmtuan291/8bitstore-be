using _8bitstore_be.Data;
using _8bitstore_be.DTO.Wishlist;
using _8bitstore_be.Interfaces;
using _8bitstore_be.Models;
using Microsoft.EntityFrameworkCore;

namespace _8bitstore_be.Services
{
    public class WishlistService: IWishlistService
    {
        private readonly _8bitstoreContext _context;

        public WishlistService(_8bitstoreContext context)
        {
            _context = context;
        }

        public async Task<WishlistDto?> GetWishlistAsync(string userId)
        {
            var wishlist = await _context.Wishlists
                .Where(w => w.UserId == userId)
                .Include(w => w.Products)
                    .ThenInclude(wishlistItem => wishlistItem.Product)
                .SingleOrDefaultAsync();

            if (wishlist == null)
            {
                return null;
            }

            return new WishlistDto()
            {
                wishlistItems = wishlist.Products
                    .Select(item => new WishlistItemDto
                    {
                        ImgUrl = item.Product.ImgUrl,
                        ProductId = item.Product.ProductID,
                        ProductName = item.Product.ProductName,
                        price = item.Product.Price,
                    })
                    .ToList()
            };
        }

        public async Task AddItemAsync(string productId, string userId)
        {
            var wishlist = await _context.Wishlists
                .Where(w => w.UserId == userId)
                .Include(w => w.Products)
                .SingleOrDefaultAsync();

            var product = await _context.Products
                .Where(p => p.ProductID == productId)
                .SingleOrDefaultAsync();

            if (product == null)
            {
                throw new KeyNotFoundException("Cannot find the product.");
            }

            if (wishlist == null)
            {
                wishlist = new Wishlist()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    Products = new List<WishlistItem>()
                };
               await _context.Wishlists.AddAsync(wishlist);
            }

            var wishlistItem = wishlist.Products
                .Where(p => p.ProductId == productId)
                .FirstOrDefault();

            if (wishlistItem == null)
            {
                await _context.WishlistItems.AddAsync(new WishlistItem
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductId = product.ProductID,
                    WishlistId = wishlist.Id,
                });
            }
            
            await _context.SaveChangesAsync();
        }

        public async Task RemoveItemAsync(string userId, string productId)
        {
            var wishlist = await _context.Wishlists
               .Where(w => w.UserId == userId)
               .Include(w => w.Products)
               .SingleOrDefaultAsync();

            var product = await _context.Products
                .Where(p => p.ProductID == productId)
                .SingleOrDefaultAsync();

            if (product == null || wishlist == null)
            {
                throw new KeyNotFoundException("Cannot find the product or wishlist.");
            }

            var wishlistItem = wishlist.Products
                .Where(p => p.ProductId == productId)
                .FirstOrDefault();
             
            if (wishlistItem != null)
            {
                wishlist.Products.Remove(wishlistItem);
            }

            await _context.SaveChangesAsync();
        }
    }
}
 