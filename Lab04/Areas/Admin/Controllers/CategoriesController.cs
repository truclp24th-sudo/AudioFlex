using Microsoft.AspNetCore.Mvc;
using AudioFlex.Models;
using AudioFlex.Repositories.Interfaces;

namespace AudioFlex.Areas.Admin.Controllers
{
    /// <summary>
    /// Quản lý CRUD Danh mục sản phẩm trong khu vực Admin.
    /// </summary>
    [Area("Admin")]
    public class CategoriesController : AdminBaseController
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // GET: Admin/Categories
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Danh sách danh mục";
            ViewData["ActiveMenu"] = "Categories";
            var categories = await _categoryRepository.GetAllWithProductsAsync();
            return View(categories);
        }

        // GET: Admin/Categories/Create
        public IActionResult Create()
        {
            ViewData["Title"] = "Thêm danh mục";
            ViewData["ActiveMenu"] = "Categories";
            return View();
        }

        // POST: Admin/Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            ViewData["Title"] = "Thêm danh mục";
            ViewData["ActiveMenu"] = "Categories";

            if (!ModelState.IsValid)
            {
                return View(category);
            }

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();

            TempData["Success"] = "Thêm danh mục thành công!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Categories/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Title"] = "Sửa danh mục";
            ViewData["ActiveMenu"] = "Categories";

            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        // POST: Admin/Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            ViewData["Title"] = "Sửa danh mục";
            ViewData["ActiveMenu"] = "Categories";

            if (id != category.CategoryId) return NotFound();

            if (!ModelState.IsValid)
            {
                return View(category);
            }

            _categoryRepository.Update(category);
            await _categoryRepository.SaveChangesAsync();

            TempData["Success"] = "Cập nhật danh mục thành công!";
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            // Không cho xóa danh mục nếu vẫn còn sản phẩm thuộc danh mục đó
            if (await _categoryRepository.HasProductsAsync(id))
            {
                TempData["Error"] = "Không thể xóa danh mục vì vẫn còn sản phẩm thuộc danh mục này!";
                return RedirectToAction(nameof(Index));
            }

            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return NotFound();

            _categoryRepository.Remove(category);
            await _categoryRepository.SaveChangesAsync();

            TempData["Success"] = "Xóa danh mục thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}
