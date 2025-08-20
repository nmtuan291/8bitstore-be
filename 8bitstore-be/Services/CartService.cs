using _8bitstore_be.DTO.Cart;
using _8bitstore_be.Interfaces.Services;
using _8bitstore_be.Interfaces.Repositories;
using _8bitstore_be.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _8bitstore_be.Exceptions;

namespace _8bitstore_be.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CartService> _logger;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository,  ILogger<CartService> logger)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task AddItemAsync(string userId, string productId, int quantity)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new UserNotFoundException(userId);
                
                if (string.IsNullOrEmpty(productId))
                    throw new ProductNotFoundException(productId);
                
                if (quantity <= 0)
                    throw new ProductQuantityException(productId);

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
                    var product = await _productRepository.GetByIdAsync(productId);
                    if (product.StockNum < quantity)
                        throw new ProductQuantityException(productId);
                    
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
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to add item to cart for user {UserId}", userId);
                throw;
            }
        }

        public async Task DeleteItemAsync(string userId, string productId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new UserNotFoundException(userId);
                
            if (string.IsNullOrEmpty(productId))
                throw new ProductNotFoundException(productId);

            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
                throw new CartNotFoundException();
                
            var itemToRemove = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
                
            if (itemToRemove == null)
                throw new CartItemNotFoundException();
                
            cart.CartItems.Remove(itemToRemove);
            await _cartRepository.SaveChangesAsync();
        }

        public async Task EmptyCartAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new UserNotFoundException(userId);

            await _cartRepository.EmptyCartAsync(userId);
        }

        public async Task<CartDto> GetCartAsync(string userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
                throw new CartNotFoundException();
            
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
