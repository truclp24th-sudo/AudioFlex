using Microsoft.EntityFrameworkCore;
using SpicyKorea.Data;
using SpicyKorea.Models;
using SpicyKorea.Repositories.Interfaces;

namespace SpicyKorea.Repositories.Implementations
{
    public class BlogRepository : GenericRepository<Blog>, IBlogRepository
    {
        public BlogRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Blog>> GetLatestAsync(int count)
        {
            return await _dbSet
                .OrderByDescending(b => b.CreatedDate)
                .Take(count)
                .ToListAsync();
        }
    }
}
