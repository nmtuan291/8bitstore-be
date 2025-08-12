using _8bitstore_be.DTO.Wishlist;
using _8bitstore_be.Interfaces.Services;
using _8bitstore_be.Interfaces.Repositories;
using _8bitstore_be.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _8bitstore_be.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _wishlistRepository;
        private readonly IProductRepository _productRepository;

        public WishlistService(IWishlistRepository wishlistRepository, IProductRepository productRepository)
        {
            _wishlistRepository = wishlistRepository;
            _productRepository = productRepository;
        }

        public async Task<WishlistDto?> GetWishlistAsync(string userId)
        {
            var wishlist = await _wishlistRepository.GetWishlistByUserIdAsync(userId);
            if (wishlist == null)
                return null;
            return new WishlistDto
            {
                wishlistItems = wishlist.Products.Select(item => new WishlistItemDto
                {
                    ImgUrl = item.Product?.ImgUrl?.ToList(),
                    ProductId = item.Product?.ProductID,
                    ProductName = item.Product?.ProductName,
                    Price = item.Product?.Price ?? 0,
                }).ToList()
            };
        }

        public async Task<bool> AddItemAsync(string productId, string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(productId) || string.IsNullOrEmpty(userId))
                    return false;

                var wishlist = await _wishlistRepository.GetWishlistByUserIdAsync(userId);
                var product = (await _productRepository.FindAsync(p => p.ProductID == productId)).FirstOrDefault();
                if (product == null)
                    return false;
                if (wishlist == null)
                {
                    wishlist = new Wishlist
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = userId,
                        Products = new List<WishlistItem>()
                    };
                    await _wishlistRepository.AddAsync(wishlist);
                }
                var wishlistItem = wishlist.Products.FirstOrDefault(p => p.ProductId == productId);
                if (wishlistItem == null)
                {
                    wishlist.Products.Add(new WishlistItem
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductId = product.ProductID,
                        WishlistId = wishlist.Id,
                    });
                }
                await _wishlistRepository.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveItemAsync(string userId, string productId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(productId))
                    return false;

                var wishlist = await _wishlistRepository.GetWishlistByUserIdAsync(userId);
                var product = (await _productRepository.FindAsync(p => p.ProductID == productId)).FirstOrDefault();
                if (product == null || wishlist == null)
                    return false;
                var wishlistItem = wishlist.Products.FirstOrDefault(p => p.ProductId == productId);
                if (wishlistItem != null)
                {
                    wishlist.Products.Remove(wishlistItem);
                }
                await _wishlistRepository.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
 