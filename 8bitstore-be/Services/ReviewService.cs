using _8bitstore_be.DTO.Review;
using _8bitstore_be.Interfaces.Services;
using _8bitstore_be.Interfaces.Repositories;
using _8bitstore_be.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _8bitstore_be.Exceptions;

namespace _8bitstore_be.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IRepository<User> _userRepository;
        private readonly ILogger<ReviewService> _logger;
        private readonly IProductService _productService;

        public ReviewService(IReviewRepository reviewRepository, IRepository<User> userRepository, 
            ILogger<ReviewService> logger,  IProductService productService)
        {
            _reviewRepository = reviewRepository;
            _userRepository = userRepository;
            _logger = logger;
            _productService = productService;
        }

        public async Task<List<ReviewDto>> GetReviewAsync(string productId)
        {
            if (await _productService.GetProductAsync(productId) == null) 
                throw new ProductNotFoundException(productId);
            
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
            if (await _userRepository.GetByIdAsync(userId) == null)
                throw new UserNotFoundException(userId);
            
            if (await _productService.GetProductAsync(review.ProductId) == null)
                throw new ProductNotFoundException(review.ProductId);
                
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
    }
}
