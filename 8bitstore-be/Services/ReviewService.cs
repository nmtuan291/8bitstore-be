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

        public ReviewService(IReviewRepository reviewRepository, IRepository<User> userRepository)
        {
            _reviewRepository = reviewRepository;
            _userRepository = userRepository;
        }

        public async Task<ICollection<ReviewDto>> GetReviewAsync(string productId)
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

        public async Task AddReviewAsync(string userId, ReviewDto review)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User does not exists");
            try
            {
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
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to save review", ex);
            }
        }
    }
}
