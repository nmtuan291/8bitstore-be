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

            if (userId == null)
            {
                return NotFound("Cannot find the user");
            }

            try
            {
                var wishlist = await _wishlistService.GetWishlistAsync(userId);
                return Ok(wishlist);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        
        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddItem([FromBody] string productId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;

            if (userId == null)
            {
                return BadRequest("User does not exists");
            }

            try
            {
                await _wishlistService.AddItemAsync(productId, userId);
                return Ok("Add item to wishlist successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> RemoveItem([FromQuery] string productId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
            
            if (userId == null)
            {
                return BadRequest("User does not exists");
            }

            try
            {
                await _wishlistService.RemoveItemAsync(userId, productId);
                return Ok("Remove item successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
