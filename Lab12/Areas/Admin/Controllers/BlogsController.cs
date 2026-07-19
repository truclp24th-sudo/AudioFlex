using Microsoft.AspNetCore.Mvc;
using AudioFlex.Models;
using AudioFlex.Repositories.Interfaces;

namespace AudioFlex.Areas.Admin.Controllers
{
    /// <summary>
    /// Quản lý CRUD Tin tức (Blog) trong khu vực Admin.
    /// </summary>
    [Area("Admin")]
    public class BlogsController : AdminBaseController
    {
        private readonly IBlogRepository _blogRepository;

        public BlogsController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        // GET: Admin/Blogs
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Danh sách tin tức";
            ViewData["ActiveMenu"] = "Blogs";
            var blogs = await _blogRepository.GetAllAsync();
            return View(blogs.OrderByDescending(b => b.CreatedDate));
        }

        // GET: Admin/Blogs/Create
        public IActionResult Create()
        {
            ViewData["Title"] = "Thêm tin tức";
            ViewData["ActiveMenu"] = "Blogs";
            return View();
        }

        // POST: Admin/Blogs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog)
        {
            ViewData["Title"] = "Thêm tin tức";
            ViewData["ActiveMenu"] = "Blogs";

            if (!ModelState.IsValid)
            {
                return View(blog);
            }

            blog.CreatedDate = DateTime.Now;
            await _blogRepository.AddAsync(blog);
            await _blogRepository.SaveChangesAsync();

            TempData["Success"] = "Thêm tin tức thành công!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Blogs/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Title"] = "Sửa tin tức";
            ViewData["ActiveMenu"] = "Blogs";

            var blog = await _blogRepository.GetByIdAsync(id);
            if (blog == null) return NotFound();
            return View(blog);
        }

        // POST: Admin/Blogs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Blog blog)
        {
            ViewData["Title"] = "Sửa tin tức";
            ViewData["ActiveMenu"] = "Blogs";

            if (id != blog.BlogId) return NotFound();

            if (!ModelState.IsValid)
            {
                return View(blog);
            }

            // Giữ nguyên CreatedDate gốc, chỉ cập nhật nội dung
            var existing = await _blogRepository.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.Title = blog.Title;
            existing.Content = blog.Content;
            existing.ImagePath = blog.ImagePath;

            _blogRepository.Update(existing);
            await _blogRepository.SaveChangesAsync();

            TempData["Success"] = "Cập nhật tin tức thành công!";
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Blogs/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var blog = await _blogRepository.GetByIdAsync(id);
            if (blog == null) return NotFound();

            _blogRepository.Remove(blog);
            await _blogRepository.SaveChangesAsync();

            TempData["Success"] = "Xóa tin tức thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}
