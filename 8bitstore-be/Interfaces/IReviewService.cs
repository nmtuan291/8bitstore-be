using _8bitstore_be.DTO.Review;

namespace _8bitstore_be.Interfaces
{
    public interface IReviewService
    {
        public Task<ICollection<ReviewDto>> GetReviewAsync(string productId);
        public Task AddReviewAsync(string userId, ReviewDto review);
    }
}
