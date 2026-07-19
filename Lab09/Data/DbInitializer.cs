using AudioFlex.Models;

namespace AudioFlex.Data
{
    /// <summary>
    /// Khởi tạo dữ liệu mẫu (seed data) cho ứng dụng khi chạy lần đầu.
    /// Được gọi từ Program.cs sau khi Migrate database.
    /// </summary>
    public static class DbInitializer
    {
        public static void Seed(ApplicationDbContext context)
        {
            // Lưu ý: database đã được tạo bằng Migration (context.Database.Migrate() ở Program.cs).
            // Hàm này chỉ chèn dữ liệu mẫu nếu bảng tương ứng đang rỗng.

            // ---------- 1. Category ----------
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { CategoryName = "Mỳ Cay", Description = "Các món mì cay 7 cấp độ chuẩn vị Hàn Quốc" },
                    new Category { CategoryName = "Đồ Uống", Description = "Trà trái cây, trà sữa, matcha..." },
                    new Category { CategoryName = "Best Seller", Description = "Những món ăn bán chạy nhất quán" }
                );
                context.SaveChanges();
            }

            var miCay = context.Categories.First(c => c.CategoryName == "Mỳ Cay");
            var doUong = context.Categories.First(c => c.CategoryName == "Đồ Uống");
            var bestSeller = context.Categories.First(c => c.CategoryName == "Best Seller");

            // ---------- 2. Product ----------
            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    new Product { ProductName = "Mì cay bò", Price = 45000, ImagePath = "mì cay 1.jpg", CategoryId = miCay.CategoryId, IsNew = true, Description = "Mì cay bò thơm ngon, nước dùng đậm đà 7 cấp độ." },
                    new Product { ProductName = "Mì cay hải sản", Price = 50000, ImagePath = "picture 1.jpg", CategoryId = miCay.CategoryId, IsNew = true, Description = "Mì cay hải sản tươi ngon, đầy đặn topping." },
                    new Product { ProductName = "Mì cay xúc xích", Price = 45000, ImagePath = "picture 2.jpg", CategoryId = miCay.CategoryId, IsNew = true, Description = "Mì cay xúc xích hấp dẫn cho bữa ăn nhanh." },
                    new Product { ProductName = "Mì cay bào ngư", Price = 90000, ImagePath = "picture 3.jpg", CategoryId = miCay.CategoryId, IsNew = true, Description = "Mì cay bào ngư cao cấp, bổ dưỡng." },
                    new Product { ProductName = "Matcha Latte", Price = 35000, ImagePath = "1.jpg", CategoryId = doUong.CategoryId, IsNew = true, Description = "Matcha Latte thơm béo, giải nhiệt." },
                    new Product { ProductName = "Trà Đào", Price = 30000, ImagePath = "2.jpg", CategoryId = doUong.CategoryId, IsNew = true, Description = "Trà đào cam sả thanh mát." },
                    new Product { ProductName = "Trà Vải", Price = 35000, ImagePath = "3.jpg", CategoryId = doUong.CategoryId, IsNew = true, Description = "Trà vải ngọt dịu, thơm hương hoa quả." },
                    new Product { ProductName = "Trà Chanh Dây", Price = 40000, ImagePath = "4.jpg", CategoryId = doUong.CategoryId, IsNew = true, Description = "Trà chanh dây chua ngọt hài hòa." },
                    new Product { ProductName = "Mì cay phô mai", Price = 55000, ImagePath = "product-1.jpg", CategoryId = bestSeller.CategoryId, IsNew = false, Description = "Mì cay phô mai béo ngậy - món bán chạy nhất." },
                    new Product { ProductName = "Mì cay tôm", Price = 60000, ImagePath = "product-2.jpg", CategoryId = bestSeller.CategoryId, IsNew = false, Description = "Mì cay tôm tươi - lựa chọn yêu thích của khách hàng." }
                );
                context.SaveChanges();
            }

            // ---------- 3. Blog ----------
            if (!context.Blogs.Any())
            {
                context.Blogs.AddRange(
                    new Blog
                    {
                        Title = "Khai trương chi nhánh mới Spicy Korea",
                        Content = "Spicy Korea hân hạnh khai trương chi nhánh mới với không gian rộng rãi, hiện đại, phục vụ những tô mì cay 7 cấp độ chuẩn vị Hàn Quốc.",
                        ImagePath = "blog-1.jpg",
                        CreatedDate = DateTime.Now.AddDays(-10)
                    },
                    new Blog
                    {
                        Title = "Bí quyết pha chế nước dùng mì cay chuẩn vị",
                        Content = "Nước dùng là linh hồn của món mì cay. Cùng khám phá bí quyết pha chế nước dùng đậm đà, cay nồng đặc trưng của Spicy Korea.",
                        ImagePath = "blog-2.jpg",
                        CreatedDate = DateTime.Now.AddDays(-6)
                    },
                    new Blog
                    {
                        Title = "Ưu đãi tháng này: Giảm 20% cho hóa đơn từ 200.000đ",
                        Content = "Chương trình khuyến mãi đặc biệt dành cho khách hàng thân thiết trong tháng này. Nhanh tay ghé quán để nhận ưu đãi hấp dẫn.",
                        ImagePath = "blog-3.jpg",
                        CreatedDate = DateTime.Now.AddDays(-2)
                    }
                );
                context.SaveChanges();
            }

            // ---------- 4. Account (Admin + Customer mẫu) ----------
            if (!context.Accounts.Any())
            {
                context.Accounts.AddRange(
                    new Account
                    {
                        Username = "admin",
                        Password = "admin123", // Plain text theo yêu cầu đồ án
                        Role = "Admin",
                        FullName = "Quản trị viên",
                        Email = "admin@AudioFlex.com"
                    },
                    new Account
                    {
                        Username = "customer",
                        Password = "123456",
                        Role = "Customer",
                        FullName = "Khách hàng demo",
                        Email = "customer@AudioFlex.com"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
