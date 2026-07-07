# Spicy Korea - ASP.NET Core MVC (.NET 8)

Dự án được chuyển đổi từ website HTML/CSS/JS tĩnh sang ASP.NET Core MVC (.NET 8) + Entity Framework Core (Code First) + SQL Server.

## 1. Yêu cầu môi trường
- Visual Studio 2022 (v17.8+) có cài workload "ASP.NET and web development"
- .NET 8 SDK
- SQL Server (LocalDB / Express / Developer đều được)

## 2. Cách chạy dự án

1. Mở file `SpicyKorea.sln` bằng Visual Studio.
2. Chờ Visual Studio **Restore NuGet Packages** tự động (hoặc bấm chuột phải vào Solution → Restore NuGet Packages).
3. Mở **Tools → NuGet Package Manager → Package Manager Console**, chọn Default project là `SpicyKorea`, chạy lần lượt:
   ```
   Add-Migration InitialCreate
   Update-Database
   ```
4. Bấm **F5** (hoặc Ctrl+F5) để chạy dự án.

> Lưu ý: Chuỗi kết nối mặc định trong `appsettings.json` là:
> `Server=.;Database=SpicyKoreaDb;Trusted_Connection=True;TrustServerCertificate=True;`
> Nếu bạn dùng SQL Server instance khác (vd LocalDB), hãy sửa lại `Server=` cho phù hợp,
> ví dụ: `Server=(localdb)\\mssqllocaldb;...`

5. Chương trình sẽ tự động gọi `Database.Migrate()` và chèn dữ liệu mẫu (seed data) khi chạy lần đầu,
   nên bạn không cần tự nhập dữ liệu.

## 3. Tài khoản demo (đã được seed sẵn)

| Vai trò   | Username  | Password | Vào được |
|-----------|-----------|----------|----------|
| Admin     | admin     | admin123 | Trang người dùng + `/Admin` |
| Khách hàng| customer  | 123456   | Trang người dùng |

Đăng nhập tại: `/Account/Login`
Trang quản trị: `/Admin` (chỉ vào được khi đăng nhập với Role = Admin)

## 4. Cấu trúc dự án

```
SpicyKorea/
 ├─ Controllers/         Controller cho site người dùng (Home, Product, Blog, Contact, Account)
 ├─ Models/              Category, Product, Blog, Contact, Account, Order, OrderDetail
 ├─ ViewModels/          LoginViewModel, RegisterViewModel
 ├─ Data/                ApplicationDbContext, DbInitializer (seed data)
 ├─ Repositories/        Interfaces + Implementations (Repository Pattern)
 ├─ Views/               Razor View cho site người dùng
 ├─ Areas/Admin/         Khu vực quản trị (CRUD Product/Category/Blog, quản lý Order/User/Feedback)
 └─ wwwroot/             css, js, img, lib (giữ nguyên từ bản HTML gốc) + admin/dist (AdminLTE)
```

## 5. Ghi chú quan trọng

- **Không dùng ASP.NET Identity.** Đăng nhập tự viết, so sánh trực tiếp Username/Password (plain text)
  theo đúng yêu cầu đồ án môn học. Thông tin đăng nhập được lưu vào Session (`UserId`, `Username`, `Role`).
- Toàn bộ khu vực `/Admin` được bảo vệ bởi `AdminBaseController` (kiểm tra Session ở `OnActionExecuting`):
  chưa đăng nhập hoặc không có Role = "Admin" sẽ tự động chuyển hướng về `/Account/Login`.
- Danh mục (Category) không thể xóa nếu vẫn còn Sản phẩm thuộc danh mục đó.
- Ảnh sản phẩm/tin tức lưu tên file trong cột `ImagePath`, ứng với file vật lý trong `wwwroot/img/`.
  Khi thêm sản phẩm/tin tức mới trong Admin, hãy nhập đúng tên file ảnh đã có sẵn trong thư mục này,
  hoặc copy thêm ảnh mới vào `wwwroot/img/` rồi nhập đúng tên file.
