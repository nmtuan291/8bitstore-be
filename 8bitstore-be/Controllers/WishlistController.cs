using System.Security.Claims;
using _8bitstore_be.Interfaces;
using _8bitstore_be.Interfaces.Services;
using _8bitstore_be.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _8bitstore_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        public readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }
        
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetWishlist()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var wishlist = await _wishlistService.GetWishlistAsync(userId);
            return Ok(wishlist);
        }
        
        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddItem([FromBody] string productId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            bool success = await _wishlistService.AddItemAsync(productId, userId);
            
            if (success)
                return Ok();
            
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to add item to wishlist" });
        }
        
        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> RemoveItem(string productId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            bool success = await _wishlistService.RemoveItemAsync(userId, productId);
            
            if (success)
                return Ok();
            
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to remove item from wishlist" });
        }
    }
}
