using _8bitstore_be.DTO.Cart;
using _8bitstore_be.Interfaces.Services;
using _8bitstore_be.Interfaces.Repositories;
using _8bitstore_be.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _8bitstore_be.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<bool> AddItemAsync(string userId, string productId, int quantity)
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(productId) || quantity <= 0)
                    return false;

                var cart = await _cartRepository.GetCartByUserIdAsync(userId);
                if (cart == null)
                {
                    cart = new Cart
                    {
                        UserId = userId,
                        Id = Guid.NewGuid().ToString(),
                        CartItems = new List<CartItem>()
                    };
                    await _cartRepository.AddAsync(cart);
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
                await _cartRepository.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteItemAsync(string userId, string productId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(productId))
                    return false;

                var cart = await _cartRepository.GetCartByUserIdAsync(userId);
                if (cart == null)
                    return false;
                var itemToRemove = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
                if (itemToRemove == null)
                    return false;
                cart.CartItems.Remove(itemToRemove);
                await _cartRepository.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> EmptyCartAsync(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                    return false;

                await _cartRepository.EmptyCartAsync(userId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<CartDto> GetCartAsync(string userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
                throw new KeyNotFoundException("Cart with the user ID cannot be found");
            return new CartDto
            {
                Id = cart.Id,
                UserId = userId,
                CartItems = cart.CartItems.Select(item => new CartItemDto
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    ProductName = item.Product?.ProductName,
                    Price = item.Product?.Price ?? 0,
                    ImgUrl = item.Product?.ImgUrl?.ToList()
                }).ToList(),
            };
        }
    }
}
