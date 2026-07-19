using AudioFlex.Models;

namespace AudioFlex.Repositories.Interfaces
{
    public interface IBlogRepository : IGenericRepository<Blog>
    {
        // Lấy danh sách tin tức mới nhất (dùng cho trang chủ / trang blog)
        Task<IEnumerable<Blog>> GetLatestAsync(int count);
    }
}
