using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AudioFlex.Models;
using AudioFlex.Repositories.Interfaces;

namespace AudioFlex.Areas.Admin.Controllers
{
    /// <summary>
    /// Quản lý CRUD Sản phẩm trong khu vực Admin (Thêm/Sửa/Xóa/Xem danh sách).
    /// </summary>
    [Area("Admin")]
    public class ProductsController : AdminBaseController
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IGenericRepository<ProductImage> _productImageRepository; // Thêm repository cho ảnh phụ

        public ProductsController(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IGenericRepository<ProductImage> productImageRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _productImageRepository = productImageRepository;
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
        public async Task<IActionResult> Create(Product product, string SubImagePaths)
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

            // Xử lý lưu ảnh phụ (nếu có nhập)
            if (!string.IsNullOrEmpty(SubImagePaths))
            {
                var paths = SubImagePaths.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var path in paths)
                {
                    var newImg = new ProductImage
                    {
                        ProductId = product.ProductId,
                        ImagePath = path.Trim()
                    };
                    await _productImageRepository.AddAsync(newImg);
                }
                await _productImageRepository.SaveChangesAsync();
            }

            TempData["Success"] = "Thêm sản phẩm thành công!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Title"] = "Sửa sản phẩm";
            ViewData["ActiveMenu"] = "Products";

            // Dùng hàm có lấy kèm ảnh phụ để load lên form
            var product = await _productRepository.GetByIdWithImagesAsync(id);
            if (product == null) return NotFound();

            await LoadCategoriesAsync(product.CategoryId);

            // Chuyển danh sách ảnh phụ thành chuỗi xuống dòng để hiện lên textarea
            if (product.ProductImages != null && product.ProductImages.Any())
            {
                ViewBag.SubImagePaths = string.Join("\n", product.ProductImages.Select(i => i.ImagePath));
            }

            return View(product);
        }

        // POST: Admin/Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product, string SubImagePaths)
        {
            ViewData["Title"] = "Sửa sản phẩm";
            ViewData["ActiveMenu"] = "Products";

            if (id != product.ProductId) return NotFound();

            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync(product.CategoryId);
                return View(product);
            }

            var existing = await _productRepository.GetByIdWithImagesAsync(id);
            if (existing == null) return NotFound();

            // Cập nhật thông tin chính
            existing.ProductName = product.ProductName;
            existing.Price = product.Price;
            existing.OldPrice = product.OldPrice;
            existing.ImagePath = product.ImagePath;
            existing.Description = product.Description;
            existing.IsNew = product.IsNew;
            existing.CategoryId = product.CategoryId;

            _productRepository.Update(existing);
            await _productRepository.SaveChangesAsync();

            // --- XỬ LÝ ẢNH PHỤ ---
            // 1. Xóa hết ảnh phụ cũ của sản phẩm này
            var oldImages = await _productImageRepository.FindAsync(img => img.ProductId == id);
            foreach (var img in oldImages)
            {
                _productImageRepository.Remove(img);
            }
            await _productImageRepository.SaveChangesAsync();

            // 2. Thêm lại danh sách ảnh phụ mới từ ô textarea
            if (!string.IsNullOrEmpty(SubImagePaths))
            {
                var paths = SubImagePaths.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var path in paths)
                {
                    var newImg = new ProductImage
                    {
                        ProductId = id,
                        ImagePath = path.Trim()
                    };
                    await _productImageRepository.AddAsync(newImg);
                }
                await _productImageRepository.SaveChangesAsync();
            }
            // ---------------------

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
                TempData["Error"] = "Không thể xóa sản phẩm này vì đã tồn tại trong đơn hàng!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}