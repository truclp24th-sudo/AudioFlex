using Microsoft.EntityFrameworkCore;
using SpicyKorea.Data;
using SpicyKorea.Models;
using SpicyKorea.Repositories.Interfaces;

namespace SpicyKorea.Repositories.Implementations
{
    /// <summary>
    /// Triển khai các thao tác riêng cho Product (bao gồm cả navigation Category).
    /// </summary>
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetAllWithCategoryAsync()
        {
            return await _dbSet
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();
        }

        public async Task<Product> GetByIdWithCategoryAsync(int id)
        {
            return await _dbSet
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId)
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();
        }
    }
}
