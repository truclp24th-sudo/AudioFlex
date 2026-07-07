using Microsoft.AspNetCore.Mvc;
using SpicyKorea.Repositories.Interfaces;

namespace SpicyKorea.Areas.Admin.Controllers
{
    /// <summary>
    /// Quản lý tài khoản (Account) trong khu vực Admin: xem danh sách và xóa tài khoản khách hàng.
    /// </summary>
    [Area("Admin")]
    public class UsersController : AdminBaseController
    {
        private readonly IAccountRepository _accountRepository;

        public UsersController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        // GET: Admin/Users
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Danh sách tài khoản";
            ViewData["ActiveMenu"] = "Users";
            var accounts = await _accountRepository.GetAllAsync();
            return View(accounts.OrderBy(a => a.AccountId));
        }

        // POST: Admin/Users/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if (account == null) return NotFound();

            // Không cho xóa chính tài khoản admin đang đăng nhập
            var currentUserId = HttpContext.Session.GetInt32("UserId");
            if (currentUserId == id)
            {
                TempData["Error"] = "Không thể xóa tài khoản đang đăng nhập!";
                return RedirectToAction(nameof(Index));
            }

            _accountRepository.Remove(account);
            await _accountRepository.SaveChangesAsync();

            TempData["Success"] = "Xóa tài khoản thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}
