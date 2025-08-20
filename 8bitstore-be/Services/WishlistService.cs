using _8bitstore_be.DTO.Wishlist;
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
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _wishlistRepository;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<WishlistService> _logger;

        public WishlistService(IWishlistRepository wishlistRepository, IProductRepository productRepository,  ILogger<WishlistService> logger)
        {
            _wishlistRepository = wishlistRepository;
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<WishlistDto> GetWishlistAsync(string userId)
        {
            var wishlist = await _wishlistRepository.GetWishlistByUserIdAsync(userId);

            return new WishlistDto
            {
                wishlistItems = wishlist.Products.Select(item => new WishlistItemDto
                {
                    ImgUrl = item.Product?.ImgUrl?.ToList() ?? new List<string>(),
                    ProductId = item.Product?.ProductID ?? "",
                    ProductName = item.Product?.ProductName ?? "",
                    Price = item.Product?.Price ?? 0,
                }).ToList()
            };
        }

        public async Task AddItemAsync(string productId, string userId)
        {
            var wishlist = await _wishlistRepository.GetWishlistByUserIdAsync(userId);
            var product = (await _productRepository.FindAsync(p => p.ProductID == productId)).FirstOrDefault();
            if (product == null)
                throw new ProductNotFoundException(productId);
                
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
        }

        public async Task RemoveItemAsync(string userId, string productId)
        {
            var wishlist = await _wishlistRepository.GetWishlistByUserIdAsync(userId);
            var product = (await _productRepository.FindAsync(p => p.ProductID == productId)).FirstOrDefault();
            if (product == null)
                throw new ProductNotFoundException(productId);
            var wishlistItem = wishlist.Products.FirstOrDefault(p => p.ProductId == productId);
            if (wishlistItem != null)
            {
                wishlist.Products.Remove(wishlistItem);
            }
            await _wishlistRepository.SaveChangesAsync();
        }
    }
}
 