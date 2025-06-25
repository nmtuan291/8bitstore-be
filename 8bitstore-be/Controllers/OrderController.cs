using System.Security.Claims;
using _8bitstore_be.DTO;
using _8bitstore_be.DTO.Order;
using _8bitstore_be.Interfaces;
using _8bitstore_be.Interfaces.Services;
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

        [HttpPost("add")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto order)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;

            if (userId == null)
            {
                return BadRequest("User does not exists");
            } 

            try
            {
                await _orderService.CreateOrderAsync(order, userId);
                return Ok("Create order successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
            
            if (userId == null)
            {
                return BadRequest("User does not exits");
            }

            try
            {
                ICollection<OrderDto> orders = await _orderService.GetOrderAsync(userId);
                return Ok(new StatusResponse<ICollection<OrderDto>>
                {
                    Status = "SUCCESS",
                    Message = orders
                });
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
         }

        [HttpPatch("change-status/{orderId}")]
        public async Task<IActionResult> ChangeStatus(string orderId, [FromBody] string status)
        {
            if (orderId == null)
            {
                return BadRequest("Missing order");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                OrderDto order = new()
                {
                    OrderId = orderId,
                    Status = status
                };

                await _orderService.ChangeOrderStatusAsync(order);
                return Ok("Order's status changed successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            };
        }
    }
}
