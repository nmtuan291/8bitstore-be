using _8bitstore_be.Data;
using _8bitstore_be.DTO.Cart;
using _8bitstore_be.Interfaces;
using _8bitstore_be.Models;
using Microsoft.EntityFrameworkCore;

namespace _8bitstore_be.Services
{
    public class CartService: ICartService
    {
        private readonly _8bitstoreContext _context;

        public CartService(_8bitstoreContext context)
        {
            _context = context;
        }

        public async Task AddItemAsync(string userId, string productId, int quantity)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .SingleOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    Id = Guid.NewGuid().ToString(),
                    CartItems = new List<CartItem>()
                };

                await _context.Carts.AddAsync(cart);
            }

            var cartItem = cart.CartItems.FirstOrDefault(item => item.ProductId == productId);

            if (cartItem == null)
            {
                cart.CartItems.Add(new CartItem
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductId = productId,
                    CartId = cart.Id,
                    Quantity = quantity
                });
            }
            else
            {
                cartItem.Quantity = quantity;
            }

            await _context.SaveChangesAsync();
        }
        
        public async Task DeleteItemAsync(string userId, string productId)
        {
            var cart = await _context.Carts
                .Where(c => c.UserId == userId)
                .Include(c => c.CartItems)
                .SingleOrDefaultAsync();

            if (cart == null)
            {
                throw new KeyNotFoundException($"Cart was not found");
            }

            var itemToRemove = cart.CartItems
                .Where(ci => ci.ProductId == productId)
                .SingleOrDefault();

            if (itemToRemove == null)
            {
                throw new KeyNotFoundException("Item not found in ther cart");
            }
            cart.CartItems.Remove(itemToRemove);

            await _context.SaveChangesAsync();
        }

        public async Task<CartDto> GetCartAsync(string userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .SingleOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                throw new KeyNotFoundException("Cart with the user ID cannot be found");
            }

            return new CartDto
            {
                Id = cart.Id,
                UserId = userId,
                CartItems = cart.CartItems.Select(item => new CartItemDto
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    ProductName = item.Product.ProductName,
                    Price = item.Product.Price,
                    ImgUrl = item.Product.ImgUrl
                }).ToList(),
            };
        }
    }
}
