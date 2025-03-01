using System.Diagnostics;
using System.Security.Claims;
using _8bitstore_be.DTO.Cart;
using _8bitstore_be.Interfaces;
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

        [HttpPost("add-item")]
        public async Task<IActionResult> AddItem([FromBody] AddItemRequestDto request)  
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (userId == null || request.ProductId == null)
            {
                return BadRequest("User or product Id is missing.");
            }

            try
            {
                await _cartService.AddItemAsync(userId, request.ProductId, request.Quantity);
                return Ok("Add item to cart successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }

        [HttpDelete("delete-item")]
        public async Task<IActionResult> DeleteItem([FromQuery] string productId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _cartService.DeleteItemAsync(userId, productId);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("get-cart")]
        public async Task<IActionResult> GetCart()
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;

            if (userId == null)
            {
                return BadRequest("User ID is missing");
            }

            try
            {
                CartDto cart = await _cartService.GetCartAsync(userId);
                return Ok(cart);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
