using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using _8bitstore_be.DTO;
using _8bitstore_be.Interfaces;
using _8bitstore_be.DTO.Payment;
using _8bitstore_be.Interfaces.Services;
using _8bitstore_be.Services;

namespace _8bitstore_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;

        public PaymentController(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }
        
        [HttpPost("create-url")]
        public IActionResult CreatePaymentUrlVnpay([FromBody] PaymentRequest request)
        {
            var url = _vnPayService.CreatePaymentUrl(HttpContext, request.Amount);

            return Ok(new { result = url });
        }

        [HttpPost("save-payment-info")]
        public async Task<IActionResult> SavePaymentVnPay([FromBody] VnPayResultDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;

            if (userId == null)
            {
                return BadRequest("User ID cannot be found");
            }

            try
            {
                StatusResponse<string> response = await _vnPayService.SavePaymentAsync(request, userId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
