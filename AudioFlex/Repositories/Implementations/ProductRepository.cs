using Microsoft.EntityFrameworkCore;
using AudioFlex.Data;
using AudioFlex.Models;
using AudioFlex.Repositories.Interfaces;

namespace AudioFlex.Repositories.Implementations
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
        public async Task<IEnumerable<Product>> GetTop8LatestProductsAsync()
        {
            return await _dbSet
                .Include(p => p.Category)
                .OrderByDescending(p => p.ProductId)
                .Take(8) // Database sẽ chỉ lọc lấy 8 dòng rồi mới gửi về App
                .ToListAsync();
        }
        public async Task<Product> GetByIdWithImagesAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages) // QUAN TRỌNG: Load danh sách ảnh phụ
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }
    }
}
