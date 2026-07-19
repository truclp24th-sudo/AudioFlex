using Microsoft.AspNetCore.Mvc;
using AudioFlex.Repositories.Interfaces;

namespace AudioFlex.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FeedbacksController : AdminBaseController
    {
        private readonly IContactRepository _contactRepository;

        public FeedbacksController(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Phản hồi liên hệ";
            ViewData["ActiveMenu"] = "Feedbacks";
            var contacts = await _contactRepository.GetAllAsync();
            return View(contacts.OrderByDescending(c => c.CreatedDate));
        }

        // --- HÀM CẬP NHẬT PHẢN HỒI ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateReply(int id, string reply)
        {
            var contact = await _contactRepository.GetByIdAsync(id);
            if (contact == null) return NotFound();

            contact.AdminReply = reply;
            _contactRepository.Update(contact);
            await _contactRepository.SaveChangesAsync();

            TempData["Success"] = "Đã cập nhật phản hồi khách hàng!";
            return RedirectToAction(nameof(Index));
        }

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