using SpicyKorea.Models;

namespace SpicyKorea.Repositories.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        // Lấy toàn bộ đơn hàng kèm chi tiết (dùng cho trang quản lý đơn hàng - Admin)
        Task<IEnumerable<Order>> GetAllWithDetailsAsync();

        Task<Order> GetByIdWithDetailsAsync(int id);

        // Lấy lịch sử đơn hàng theo tài khoản (dùng cho khách hàng xem đơn của mình)
        Task<IEnumerable<Order>> GetByAccountIdAsync(int accountId);
    }
}
