using Microsoft.EntityFrameworkCore;
using SpicyKorea.Data;
using SpicyKorea.Models;
using SpicyKorea.Repositories.Interfaces;

namespace SpicyKorea.Repositories.Implementations
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> HasProductsAsync(int categoryId)
        {
            return await _context.Products.AnyAsync(p => p.CategoryId == categoryId);
        }

        public async Task<IEnumerable<Category>> GetAllWithProductsAsync()
        {
            return await _dbSet.Include(c => c.Products).ToListAsync();
        }
    }
}
