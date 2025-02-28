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

            bool response = await _cartService.AddItemAsync(userId, request.ProductId, request.Quantity);

            if (!response)
            {
                return NotFound("Cannot find product");
            }

            return Ok("Add item to cart successfully");
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

            CartDto? cart = await _cartService.GetCartAsync(userId);

            if (cart == null)
            {
                return NotFound("Cannot find cart");
            }

            return Ok(cart);
        }
    }
}
