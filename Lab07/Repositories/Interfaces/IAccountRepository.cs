using AudioFlex.Models;

namespace AudioFlex.Repositories.Interfaces
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        // Đăng nhập: so sánh trực tiếp Username/Password (KHÔNG hash) theo yêu cầu đồ án
        Task<Account> LoginAsync(string username, string password);

        // Kiểm tra trùng tên đăng nhập khi đăng ký
        Task<bool> IsUsernameExistsAsync(string username);

        Task<Account> GetByUsernameAsync(string username);
    }
}
