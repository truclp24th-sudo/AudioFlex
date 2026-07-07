using System.Linq.Expressions;

namespace SpicyKorea.Repositories.Interfaces
{
    /// <summary>
    /// Repository dùng chung cho các thao tác CRUD cơ bản, áp dụng Repository Pattern.
    /// </summary>
    /// <typeparam name="T">Kiểu entity (Product, Category, Blog, ...)</typeparam>
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
        Task<bool> SaveChangesAsync();
    }
}
