using _8bitstore_be.DTO.Review;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _8bitstore_be.Interfaces.Services
{
    public interface IReviewService
    {
        Task<List<ReviewDto>> GetReviewAsync(string productId);
        Task AddReviewAsync(string userId, ReviewDto review);
    }
} 