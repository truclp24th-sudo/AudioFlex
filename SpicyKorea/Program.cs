using Microsoft.EntityFrameworkCore;
using SpicyKorea.Data;
using SpicyKorea.Repositories.Implementations;
using SpicyKorea.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ---------- Đăng ký dịch vụ (Dependency Injection) ----------

// 1. MVC + hỗ trợ Controller cho Areas (Admin)
builder.Services.AddControllersWithViews();

// 2. EF Core - SQL Server, Code First
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Session (dùng để lưu UserId, Username, Role sau khi đăng nhập)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// 4. HttpContextAccessor - cho phép truy cập Session/HttpContext bên ngoài Controller (nếu cần)
builder.Services.AddHttpContextAccessor();

// 5. Đăng ký Repository Pattern (Scoped: 1 instance/1 request)
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

var app = builder.Build();

// ---------- Migrate Database + Seed dữ liệu mẫu khi khởi động ----------
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();   // Áp dụng các migration còn thiếu (tương đương Update-Database)
    DbInitializer.Seed(context);  // Chèn dữ liệu mẫu nếu database đang rỗng
}

// ---------- HTTP request pipeline ----------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Trang 404 tùy chỉnh (giữ nguyên giao diện 404.html gốc)
app.UseStatusCodePagesWithReExecute("/Home/NotFoundPage");

app.UseRouting();

app.UseSession(); // Phải đặt trước UseAuthorization và trước khi Controller dùng Session

app.UseAuthorization();

// Route riêng cho Areas (Admin) - phải khai báo TRƯỚC route mặc định
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

// Route mặc định cho trang người dùng
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
