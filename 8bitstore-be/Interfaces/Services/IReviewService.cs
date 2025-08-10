using _8bitstore_be.DTO.Review;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _8bitstore_be.Interfaces.Services
{
    public interface IReviewService
    {
        Task<ICollection<ReviewDto>> GetReviewAsync(string productId);
        Task<bool> AddReviewAsync(string userId, ReviewDto review);
    }
} 