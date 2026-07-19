using Microsoft.EntityFrameworkCore;
using AudioFlex.Data;
using AudioFlex.Repositories.Implementations;
using AudioFlex.Repositories.Interfaces;

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
builder.Services.AddAuthentication("AudioFlexSecurity")
    .AddCookie("AudioFlexSecurity", options =>
    {
        options.LoginPath = "/Account/Login"; // Đường dẫn trang Đăng nhập của người dùng
        options.AccessDeniedPath = "/Account/AccessDenied"; // Trang báo lỗi khi không có quyền
        options.ExpireTimeSpan = TimeSpan.FromDays(7); // Ghi nhớ đăng nhập 7 ngày
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
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

var app = builder.Build();

// ---------- Migrate Database + Seed dữ liệu mẫu khi khởi động ----------
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        context.Database.Migrate();
        DbInitializer.Seed(context);
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Không kết nối/migrate được database lúc khởi động.");
        // Không throw lại -> app vẫn start được, chỉ là DB chưa sẵn sàng
    }
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

app.UseAuthentication();

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
