using Microsoft.AspNetCore.Mvc;
using SpicyKorea.Repositories.Interfaces;

namespace SpicyKorea.Areas.Admin.Controllers
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

            ViewBag.TotalProducts = (await _productRepository.GetAllAsync()).Count();
            ViewBag.TotalCategories = (await _categoryRepository.GetAllAsync()).Count();
            ViewBag.TotalBlogs = (await _blogRepository.GetAllAsync()).Count();
            ViewBag.TotalOrders = (await _orderRepository.GetAllAsync()).Count();
            ViewBag.TotalAccounts = (await _accountRepository.GetAllAsync()).Count();
            ViewBag.TotalContacts = (await _contactRepository.GetAllAsync()).Count();

            var recentOrders = (await _orderRepository.GetAllWithDetailsAsync())
                .OrderByDescending(o => o.OrderDate)
                .Take(5);

            return View(recentOrders);
        }
    }
}
