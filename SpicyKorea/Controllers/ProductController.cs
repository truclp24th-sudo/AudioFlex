using Microsoft.AspNetCore.Mvc;
using SpicyKorea.Repositories.Interfaces;

namespace SpicyKorea.Controllers
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
            return View(products);
        }

        // GET: /Product/Details/5
        public async Task<IActionResult> Details(int id)
        {
            ViewData["ActivePage"] = "Product";
            var product = await _productRepository.GetByIdWithCategoryAsync(id);
            if (product == null) return NotFound();

            // Lấy thêm vài sản phẩm cùng danh mục để gợi ý (sản phẩm liên quan)
            var related = (await _productRepository.GetByCategoryAsync(product.CategoryId))
                .Where(p => p.ProductId != id)
                .Take(4);
            ViewBag.RelatedProducts = related;

            return View(product);
        }
    }
}
