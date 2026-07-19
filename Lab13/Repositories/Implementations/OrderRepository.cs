using Microsoft.EntityFrameworkCore;
using AudioFlex.Data;
using AudioFlex.Models;
using AudioFlex.Repositories.Interfaces;

namespace AudioFlex.Repositories.Implementations
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Order>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .Include(o => o.Account)
                .Include(o => o.OrderDetails)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<Order> GetByIdWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(o => o.Account)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.OrderId == id);
        }

        public async Task<IEnumerable<Order>> GetByAccountIdAsync(int accountId)
        {
            return await _dbSet
                .Include(o => o.OrderDetails)
                .Where(o => o.AccountId == accountId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        // Trong file OrderRepository.cs
        public async Task<IEnumerable<Order>> GetOrdersByUserIdWithDetailsAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.AccountId == userId)
                .Include(o => o.OrderDetails) // T?i danh sách chi ti?t ðõn hàng
                    .ThenInclude(od => od.Product) // T?i thông tin S?n ph?m (ð? có ImagePath)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }
    }
}
