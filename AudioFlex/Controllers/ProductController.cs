using Microsoft.AspNetCore.Mvc;
using AudioFlex.Repositories.Interfaces;

namespace AudioFlex.Controllers
{
    /// <summary>
    /// Controller cho trang Sản phẩm (product.html) của site người dùng.
    /// </summary>
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        // GET: /Product
        public async Task<IActionResult> Index()
        {
            ViewData["ActivePage"] = "Product";
            var products = await _productRepository.GetAllWithCategoryAsync();
            // 2. THÊM DÒNG NÀY: Lấy danh sách danh mục từ Database
            var categories = await _categoryRepository.GetAllAsync();

            // 3. THÊM DÒNG NÀY: Truyền danh sách danh mục sang View qua ViewBag
            ViewBag.Categories = categories;

            return View(products);
        }

        // GET: /Product/Details/5
        public async Task<IActionResult> Details(int id)
        {
            ViewData["ActivePage"] = "Product";

            // SỬA DÒNG NÀY: Gọi hàm mới có lấy kèm ProductImages
            var product = await _productRepository.GetByIdWithImagesAsync(id);

            if (product == null) return NotFound();

            // Lấy sản phẩm liên quan (giữ nguyên)
            var related = (await _productRepository.GetByCategoryAsync(product.CategoryId))
                .Where(p => p.ProductId != id)
                .Take(4);
            ViewBag.RelatedProducts = related;

            return View(product);
        }
    }
}
