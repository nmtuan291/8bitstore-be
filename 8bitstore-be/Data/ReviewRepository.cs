using _8bitstore_be.Interfaces.Repositories;
using _8bitstore_be.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _8bitstore_be.Data
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        public ReviewRepository(_8bitstoreContext context) : base(context) { }

        public async Task<List<Review>> GetReviewsByProductIdAsync(string productId)
        {
            return await _context.Reviews
                .Where(r => r.ProductId == productId)
                .Include(r => r.User)
                .ToListAsync();
        }
    }
} 