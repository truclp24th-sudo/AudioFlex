using Microsoft.AspNetCore.Mvc;
using AudioFlex.Repositories.Interfaces;

namespace AudioFlex.Controllers
{
    /// <summary>
    /// Controller cho trang Tin tức (blog.html) của site người dùng.
    /// </summary>
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogRepository;

        public BlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        // GET: /Blog
        public async Task<IActionResult> Index()
        {
            ViewData["ActivePage"] = "Blog";
            var blogs = await _blogRepository.GetAllAsync();
            return View(blogs.OrderByDescending(b => b.CreatedDate));
        }

        // GET: /Blog/Details/5
        public async Task<IActionResult> Details(int id)
        {
            ViewData["ActivePage"] = "Blog";
            var blog = await _blogRepository.GetByIdAsync(id);
            if (blog == null) return NotFound();
            return View(blog);
        }
    }
}
