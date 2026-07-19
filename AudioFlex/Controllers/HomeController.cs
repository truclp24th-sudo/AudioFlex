using Microsoft.AspNetCore.Mvc;
using AudioFlex.Models;
using AudioFlex.Repositories.Interfaces;

namespace AudioFlex.Controllers
{
    /// <summary>
    /// Controller cho các trang tĩnh của site người dùng: Trang chủ, Giới thiệu, Ưu điểm, Đánh giá.
    /// Trang chủ có lấy dữ liệu Sản phẩm/Tin tức từ SQL Server để hiển thị.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IBlogRepository _blogRepository;

        public HomeController(IProductRepository productRepository, IBlogRepository blogRepository)
        {
            _productRepository = productRepository;
            _blogRepository = blogRepository;
        }

        // GET: /  (Trang chủ)
        public async Task<IActionResult> Index()
        {
            ViewData["ActivePage"] = "Home";

            // Lấy 8 sản phẩm mới nhất và 3 tin tức mới nhất từ SQL Server để hiển thị trên trang chủ
            var products = await _productRepository.GetTop8LatestProductsAsync();
            var blogs = await _blogRepository.GetLatestAsync(3);

            ViewBag.LatestBlogs = blogs;
            return View(products);
        }

        // GET: /Home/About
        public IActionResult About()
        {
            ViewData["ActivePage"] = "About";
            return View();
        }

        // GET: /Home/Feature
        public IActionResult Feature()
        {
            ViewData["ActivePage"] = "Feature";
            return View();
        }

        // GET: /Home/Testimonial
        public IActionResult Testimonial()
        {
            ViewData["ActivePage"] = "Testimonial";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
        }

        // Trang lỗi 404 tùy chỉnh (giữ nguyên giao diện 404.html gốc)
        public IActionResult NotFoundPage()
        {
            Response.StatusCode = 404;
            return View("NotFoundPage");
        }
    }
}
