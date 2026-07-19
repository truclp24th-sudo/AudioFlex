using AudioFlex.Models;

namespace AudioFlex.Repositories.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        // Kiểm tra danh mục có đang được sản phẩm nào sử dụng không (trước khi xóa)
        Task<bool> HasProductsAsync(int categoryId);

        // Lấy danh mục kèm danh sách sản phẩm (dùng để hiển thị số lượng sản phẩm trong Admin)
        Task<IEnumerable<Category>> GetAllWithProductsAsync();
    }
}
