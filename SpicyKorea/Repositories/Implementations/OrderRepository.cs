using Microsoft.EntityFrameworkCore;
using SpicyKorea.Data;
using SpicyKorea.Models;
using SpicyKorea.Repositories.Interfaces;

namespace SpicyKorea.Repositories.Implementations
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
    }
}
