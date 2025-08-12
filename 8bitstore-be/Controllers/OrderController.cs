using System.Security.Claims;
using _8bitstore_be.DTO;
using _8bitstore_be.DTO.Order;
using _8bitstore_be.Interfaces;
using _8bitstore_be.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _8bitstore_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        
        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> CreateOrder(OrderDto order)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            } 

            bool success = await _orderService.CreateOrderAsync(order, userId);
            if (success)
                return Ok(new { message = "Order created successfully" });
            
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Order creation failed" });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            List<OrderDto> orders = await _orderService.GetOrderAsync(userId);
            return Ok(new StatusResponse<List<OrderDto>>
            {
                Status = "SUCCESS",
                Message = orders
            });
         }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllOrders()
        {
            List<OrderDto> orders = await _orderService.GetOrdersAsync();
            return Ok(new StatusResponse<List<OrderDto>>
            {
                Status = "SUCCESS",
                Message = orders
            });
        }
        
        [Authorize]   
        [HttpPatch("change-status/{orderId}")]
        public async Task<IActionResult> ChangeStatus(string orderId, [FromBody] string status)
        {

            OrderDto order = new()
            {
                OrderId = orderId,
                Status = status
            };

            await _orderService.ChangeOrderStatusAsync(order);
            return Ok();
        }
    }
}
