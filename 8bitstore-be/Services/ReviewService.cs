using _8bitstore_be.DTO.Review;
using _8bitstore_be.Interfaces.Services;
using _8bitstore_be.Interfaces.Repositories;
using _8bitstore_be.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _8bitstore_be.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IRepository<User> _userRepository;
        private readonly ILogger<ReviewService> _logger;

        public ReviewService(IReviewRepository reviewRepository, IRepository<User> userRepository, ILogger<ReviewService> logger)
        {
            _reviewRepository = reviewRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<List<ReviewDto>> GetReviewAsync(string productId)
        {
            var reviews = await _reviewRepository.GetReviewsByProductIdAsync(productId);
            return reviews.Select(review => new ReviewDto
            {
                ProductId = productId,
                UserName = review.User?.UserName,
                Comment = review.Comment,
                ReviewDate = review.ReviewDate,
                Score = review.Score
            }).ToList();
        }

        public async Task<bool> AddReviewAsync(string userId, ReviewDto review)
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(review.ProductId))
                {
                    _logger.LogError($"Invalid parameters for {nameof(AddReviewAsync)}");
                    return false;
                }
                
                Review newReview = new Review
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    Comment = review.Comment.Trim(),
                    Score = review.Score,
                    ProductId = review.ProductId,
                    ReviewDate = DateTime.UtcNow,
                };
                await _reviewRepository.AddAsync(newReview);
                await _reviewRepository.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to add review");
                return false;
            }
        }
    }
}
