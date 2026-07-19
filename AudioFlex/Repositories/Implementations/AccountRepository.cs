using Microsoft.EntityFrameworkCore;
using AudioFlex.Data;
using AudioFlex.Models;
using AudioFlex.Repositories.Interfaces;

namespace AudioFlex.Repositories.Implementations
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository(ApplicationDbContext context) : base(context)
        {
        }

        // So sánh trực tiếp username == Username && password == Password (plain text)
        public async Task<Account> LoginAsync(string username, string password)
        {
            return await _dbSet.FirstOrDefaultAsync(a =>
                a.Username == username && a.Password == password);
        }

        public async Task<bool> IsUsernameExistsAsync(string username)
        {
            return await _dbSet.AnyAsync(a => a.Username == username);
        }

        public async Task<Account> GetByUsernameAsync(string username)
        {
            return await _dbSet.FirstOrDefaultAsync(a => a.Username == username);
        }
    }
}
