using System.Diagnostics;
using System.Security.Claims;
using _8bitstore_be.DTO.Cart;
using _8bitstore_be.Interfaces;
using _8bitstore_be.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _8bitstore_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        
        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddItem(AddItemRequestDto request)  
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            await _cartService.AddItemAsync(userId, request.ProductId, request.Quantity);
            return Ok();
        }
        
        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteItem(string? productId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
            if (string.IsNullOrEmpty(userId))
                return  Unauthorized();

            bool success;
            if (!string.IsNullOrEmpty(productId))
                await _cartService.DeleteItemAsync(userId, productId);
            else
                await _cartService.EmptyCartAsync(userId);

            return Ok();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            CartDto cart = await _cartService.GetCartAsync(userId);
            return Ok(cart);
        }
    }
}
