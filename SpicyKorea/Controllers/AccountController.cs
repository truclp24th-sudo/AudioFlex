using Microsoft.AspNetCore.Mvc;
using SpicyKorea.Models;
using SpicyKorea.Repositories.Interfaces;
using SpicyKorea.ViewModels;

namespace SpicyKorea.Controllers
{
    /// <summary>
    /// Xử lý đăng nhập/đăng ký/đăng xuất dùng chung cho Admin và Customer.
    /// Theo yêu cầu đồ án: KHÔNG dùng ASP.NET Identity, KHÔNG hash mật khẩu,
    /// đăng nhập chỉ so sánh trực tiếp Username/Password, lưu thông tin vào Session.
    /// </summary>
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        // GET: /Account/Login
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ActivePage"] = "Login";
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            ViewData["ActivePage"] = "Login";

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // So sánh trực tiếp username == Username && password == Password (KHÔNG hash)
            var account = await _accountRepository.LoginAsync(model.Username, model.Password);

            if (account == null)
            {
                ModelState.AddModelError(string.Empty, "Sai tên đăng nhập hoặc mật khẩu!");
                return View(model);
            }

            // Lưu thông tin đăng nhập vào Session
            HttpContext.Session.SetInt32("UserId", account.AccountId);
            HttpContext.Session.SetString("Username", account.Username);
            HttpContext.Session.SetString("Role", account.Role);

            // Nếu là Admin -> vào trang quản trị; ngược lại về trang chủ (hoặc ReturnUrl)
            if (account.Role == "Admin")
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }

            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Register
        public IActionResult Register()
        {
            ViewData["ActivePage"] = "Login";
            return View(new RegisterViewModel());
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            ViewData["ActivePage"] = "Login";

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await _accountRepository.IsUsernameExistsAsync(model.Username))
            {
                ModelState.AddModelError(nameof(model.Username), "Tên đăng nhập đã tồn tại!");
                return View(model);
            }

            var account = new Account
            {
                Username = model.Username,
                Password = model.Password, // Plain text theo yêu cầu đồ án
                FullName = model.FullName,
                Email = model.Email,
                Role = "Customer"
            };

            await _accountRepository.AddAsync(account);
            await _accountRepository.SaveChangesAsync();

            TempData["RegisterSuccess"] = "Đăng ký thành công! Vui lòng đăng nhập.";
            return RedirectToAction(nameof(Login));
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
