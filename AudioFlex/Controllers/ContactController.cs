using Microsoft.AspNetCore.Mvc;
using AudioFlex.Models;
using AudioFlex.Repositories.Interfaces;

namespace AudioFlex.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactRepository _contactRepository;

        public ContactController(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        // GET: /Contact
        public async Task<IActionResult> Index()
        {
            ViewData["ActivePage"] = "Contact";

            // Lấy danh sách tất cả các liên hệ/phản hồi để hiển thị công khai ở dưới trang
            // Sắp xếp theo ngày mới nhất lên đầu
            var feedbacks = await _contactRepository.GetAllAsync();
            ViewBag.FeedbackList = feedbacks.OrderByDescending(c => c.CreatedDate);

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
                // Nếu lỗi, vẫn phải load lại danh sách phản hồi để View không bị lỗi Null
                var feedbacks = await _contactRepository.GetAllAsync();
                ViewBag.FeedbackList = feedbacks.OrderByDescending(c => c.CreatedDate);
                return View(contact);
            }

            // Gán thông tin mặc định
            contact.CreatedDate = DateTime.Now;
            // Mặc định Subject có thể để trống hoặc gán một giá trị mặc định vì mình đã ẩn ở giao diện
            if (string.IsNullOrEmpty(contact.Subject)) contact.Subject = "Liên hệ từ khách hàng";

            await _contactRepository.AddAsync(contact);
            await _contactRepository.SaveChangesAsync();

            TempData["ContactSuccess"] = "Gửi tin nhắn thành công! Chúng tôi sẽ liên hệ lại với bạn sớm nhất.";
            return RedirectToAction(nameof(Index));
        }
    }
}