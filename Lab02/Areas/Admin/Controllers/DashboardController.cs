using Microsoft.AspNetCore.Mvc;
using AudioFlex.Repositories.Interfaces;

namespace AudioFlex.Areas.Admin.Controllers
{
    /// <summary>
    /// Trang chủ (Dashboard) của khu vực Admin - hiển thị thống kê tổng quan.
    /// </summary>
    [Area("Admin")]
    public class DashboardController : AdminBaseController
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBlogRepository _blogRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IContactRepository _contactRepository;

        public DashboardController(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IBlogRepository blogRepository,
            IOrderRepository orderRepository,
            IAccountRepository accountRepository,
            IContactRepository contactRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _blogRepository = blogRepository;
            _orderRepository = orderRepository;
            _accountRepository = accountRepository;
            _contactRepository = contactRepository;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Trang chủ Admin";
            ViewData["ActiveMenu"] = "Dashboard";

            ViewBag.TotalProducts = await _productRepository.CountAsync();
            ViewBag.TotalCategories = await _categoryRepository.CountAsync();
            ViewBag.TotalBlogs = await _blogRepository.CountAsync();
            ViewBag.TotalOrders = await _orderRepository.CountAsync();
            ViewBag.TotalAccounts = await _accountRepository.CountAsync();
            ViewBag.TotalContacts = await _contactRepository.CountAsync();

            var recentOrders = (await _orderRepository.GetAllWithDetailsAsync())
                .OrderByDescending(o => o.OrderDate)
                .Take(5);
            // --- CHỈ THÊM ĐOẠN NÀY ĐỂ THỐNG KÊ DOANH THU ---
            var allOrders = await _orderRepository.GetAllAsync();
            ViewBag.MonthlyRevenue = allOrders
                .Where(o => o.Status != "Cancelled") // Không tính đơn bị hủy
                .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
                .Select(g => new {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Revenue = g.Sum(o => o.TotalAmount) // Lưu ý: 'TotalAmount' là tên cột tiền trong file Order.cs của bạn
                })
                .OrderByDescending(x => x.Year).ThenByDescending(x => x.Month)
                .Take(12)
                .ToList();
            return View(recentOrders);
        }

    }
}
