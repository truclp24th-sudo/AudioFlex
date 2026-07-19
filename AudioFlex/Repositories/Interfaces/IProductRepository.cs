using AudioFlex.Models;

namespace AudioFlex.Repositories.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        // Lấy sản phẩm kèm thông tin danh mục (dùng navigation property)
        Task<IEnumerable<Product>> GetAllWithCategoryAsync();

        Task<Product> GetByIdWithCategoryAsync(int id);

        Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> GetTop8LatestProductsAsync();
        Task<Product> GetByIdWithImagesAsync(int id);
    }
}
