using System.Security.Claims;
using _8bitstore_be.DTO.Review;
using _8bitstore_be.Interfaces;
using _8bitstore_be.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _8bitstore_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<IActionResult> GetReviews(string productId)
        {

            var reviews = await _reviewService.GetReviewAsync(productId);
            return Ok(reviews);
        }
        
        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddReview(ReviewDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            await _reviewService.AddReviewAsync(userId, request);
            
            return Ok();
        }
    }
}
