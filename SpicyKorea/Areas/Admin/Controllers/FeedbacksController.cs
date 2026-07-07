using Microsoft.AspNetCore.Mvc;
using SpicyKorea.Repositories.Interfaces;

namespace SpicyKorea.Areas.Admin.Controllers
{
    /// <summary>
    /// Xem và quản lý các phản hồi/liên hệ (Contact) do khách hàng gửi từ trang Liên hệ.
    /// </summary>
    [Area("Admin")]
    public class FeedbacksController : AdminBaseController
    {
        private readonly IContactRepository _contactRepository;

        public FeedbacksController(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        // GET: Admin/Feedbacks
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Phản hồi liên hệ";
            ViewData["ActiveMenu"] = "Feedbacks";
            var contacts = await _contactRepository.GetAllAsync();
            return View(contacts.OrderByDescending(c => c.CreatedDate));
        }

        // POST: Admin/Feedbacks/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var contact = await _contactRepository.GetByIdAsync(id);
            if (contact == null) return NotFound();

            _contactRepository.Remove(contact);
            await _contactRepository.SaveChangesAsync();

            TempData["Success"] = "Xóa phản hồi thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}
