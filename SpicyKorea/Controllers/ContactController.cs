using Microsoft.AspNetCore.Mvc;
using SpicyKorea.Models;
using SpicyKorea.Repositories.Interfaces;

namespace SpicyKorea.Controllers
{
    /// <summary>
    /// Controller cho trang Liên hệ (contact.html). Form liên hệ được lưu vào bảng Contact (SQL Server)
    /// thay vì localStorage như bản gốc.
    /// </summary>
    public class ContactController : Controller
    {
        private readonly IContactRepository _contactRepository;

        public ContactController(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        // GET: /Contact
        public IActionResult Index()
        {
            ViewData["ActivePage"] = "Contact";
            return View(new Contact());
        }

        // POST: /Contact
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Contact contact)
        {
            ViewData["ActivePage"] = "Contact";

            if (!ModelState.IsValid)
            {
                return View(contact);
            }

            contact.CreatedDate = DateTime.Now;
            await _contactRepository.AddAsync(contact);
            await _contactRepository.SaveChangesAsync();

            TempData["ContactSuccess"] = "Gửi tin nhắn thành công! Chúng tôi sẽ liên hệ lại với bạn sớm nhất.";
            return RedirectToAction(nameof(Index));
        }
    }
}
