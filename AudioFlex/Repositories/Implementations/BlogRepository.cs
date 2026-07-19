using Microsoft.EntityFrameworkCore;
using AudioFlex.Data;
using AudioFlex.Models;
using AudioFlex.Repositories.Interfaces;

namespace AudioFlex.Repositories.Implementations
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
