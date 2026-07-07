using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SpicyKorea.Models;
using SpicyKorea.Repositories.Interfaces;

namespace SpicyKorea.Areas.Admin.Controllers
{
    /// <summary>
    /// Quản lý CRUD Sản phẩm trong khu vực Admin (Thêm/Sửa/Xóa/Xem danh sách).
    /// </summary>
    [Area("Admin")]
    public class ProductsController : AdminBaseController
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductsController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        // Đưa danh sách danh mục ra dropdown cho Create/Edit
        private async Task LoadCategoriesAsync(int? selectedId = null)
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName", selectedId);
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Danh sách sản phẩm";
            ViewData["ActiveMenu"] = "Products";
            var products = await _productRepository.GetAllWithCategoryAsync();
            return View(products);
        }

        // GET: Admin/Products/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Title"] = "Thêm sản phẩm";
            ViewData["ActiveMenu"] = "Products";
            await LoadCategoriesAsync();
            return View();
        }

        // POST: Admin/Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            ViewData["Title"] = "Thêm sản phẩm";
            ViewData["ActiveMenu"] = "Products";

            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync(product.CategoryId);
                return View(product);
            }

            product.CreatedDate = DateTime.Now;
            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();

            TempData["Success"] = "Thêm sản phẩm thành công!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Title"] = "Sửa sản phẩm";
            ViewData["ActiveMenu"] = "Products";

            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();

            await LoadCategoriesAsync(product.CategoryId);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            ViewData["Title"] = "Sửa sản phẩm";
            ViewData["ActiveMenu"] = "Products";

            if (id != product.ProductId) return NotFound();

            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync(product.CategoryId);
                return View(product);
            }

            // Lấy bản ghi gốc để giữ nguyên CreatedDate, chỉ cập nhật các trường cho phép sửa
            var existing = await _productRepository.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.ProductName = product.ProductName;
            existing.Price = product.Price;
            existing.OldPrice = product.OldPrice;
            existing.ImagePath = product.ImagePath;
            existing.Description = product.Description;
            existing.IsNew = product.IsNew;
            existing.CategoryId = product.CategoryId;

            _productRepository.Update(existing);
            await _productRepository.SaveChangesAsync();

            TempData["Success"] = "Cập nhật sản phẩm thành công!";
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Products/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();

            try
            {
                _productRepository.Remove(product);
                await _productRepository.SaveChangesAsync();
                TempData["Success"] = "Xóa sản phẩm thành công!";
            }
            catch (Exception)
            {
                // Sản phẩm đã có trong đơn hàng -> không thể xóa do ràng buộc khóa ngoại (Restrict)
                TempData["Error"] = "Không thể xóa sản phẩm này vì đã tồn tại trong đơn hàng!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
