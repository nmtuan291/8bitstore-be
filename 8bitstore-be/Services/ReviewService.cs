using _8bitstore_be.Data;
using _8bitstore_be.DTO.Review;
using _8bitstore_be.Interfaces;
using _8bitstore_be.Models;
using Microsoft.EntityFrameworkCore;

namespace _8bitstore_be.Services
{
    public class ReviewService : IReviewService
    {
        private readonly _8bitstoreContext _context;

        public ReviewService(_8bitstoreContext context)
        {
            _context = context;
        }

        public async Task<ICollection<ReviewDto>> GetReviewAsync(string productId)
        {
            var reviews = await _context.Reviews
                   .Where(review => review.ProductId == productId)
                   .Include(review => review.User)
                   .ToListAsync();

            ICollection<ReviewDto> reviewDto = reviews.Select(review => new ReviewDto
            {
                ProductId = productId,
                UserName = review.User.UserName,
                Comment = review.Comment,
                ReviewDate = review.ReviewDate,
                Score = review.Score
            }).ToList();

            return reviewDto;
        }

        public async Task AddReviewAsync(string userId, ReviewDto review)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException("User does not exists");
            }

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

                await _context.Reviews.AddAsync(newReview);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Failed to save review", ex);
            }
        }
    }
}
