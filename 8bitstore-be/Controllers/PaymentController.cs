﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using _8bitstore_be.DTO;
using _8bitstore_be.Interfaces;
using _8bitstore_be.DTO.Payment;
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
        public IActionResult CreatePaymentUrlVnpay()
        {
            var url = _vnPayService.CreatePaymentUrl(HttpContext);

            return Ok(url);
        }

        [HttpPost("save-payment-info")]
        public async Task<IActionResult> SavePayment([FromBody] VnPayResultDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;

            try
            {
                await _vnPayService.savePaymentAsync(request, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal error");
            }
        }

    }
}
