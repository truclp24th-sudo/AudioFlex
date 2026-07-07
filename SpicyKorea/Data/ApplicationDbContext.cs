using Microsoft.EntityFrameworkCore;
using SpicyKorea.Models;

namespace SpicyKorea.Data
{
    /// <summary>
    /// EF Core DbContext - Code First. Chứa toàn bộ DbSet của ứng dụng.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ----- Category (1) - Product (N) -----
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // Không cho xóa danh mục còn sản phẩm

            // ----- Account (1) - Order (N) -----
            modelBuilder.Entity<Account>()
                .HasMany(a => a.Orders)
                .WithOne(o => o.Account)
                .HasForeignKey(o => o.AccountId)
                .OnDelete(DeleteBehavior.SetNull); // Xóa tài khoản không xóa lịch sử đơn hàng

            // ----- Order (1) - OrderDetail (N) -----
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderDetails)
                .WithOne(od => od.Order)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // Xóa đơn hàng thì xóa luôn chi tiết

            // ----- Product (1) - OrderDetail (N) -----
            modelBuilder.Entity<Product>()
                .HasMany(p => p.OrderDetails)
                .WithOne(od => od.Product)
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // Tránh multiple cascade path trên SQL Server

            // Ràng buộc unique cho Username
            modelBuilder.Entity<Account>()
                .HasIndex(a => a.Username)
                .IsUnique();
        }
    }
}
