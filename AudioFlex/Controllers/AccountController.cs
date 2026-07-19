using Microsoft.AspNetCore.Mvc;
using AudioFlex.Models;
using AudioFlex.Repositories.Interfaces;
using AudioFlex.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AudioFlex.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ActivePage"] = "Login";
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            ViewData["ActivePage"] = "Login";

            if (!ModelState.IsValid) return View(model);

            var account = await _accountRepository.LoginAsync(model.Username, model.Password);

            if (account == null)
            {
                ModelState.AddModelError(string.Empty, "Sai tên đăng nhập hoặc mật khẩu!");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.Username),
                new Claim(ClaimTypes.Role, account.Role),
                new Claim("UserId", account.AccountId.ToString())
            };

            // --- SỬA TẠI ĐÂY: Dùng "AudioFlexSecurity" ---
            var claimsIdentity = new ClaimsIdentity(claims, "AudioFlexSecurity");

            // --- SỬA TẠI ĐÂY: Dùng "AudioFlexSecurity" ---
            await HttpContext.SignInAsync("AudioFlexSecurity",
                new ClaimsPrincipal(claimsIdentity));

            HttpContext.Session.SetInt32("UserId", account.AccountId);
            HttpContext.Session.SetString("Username", account.Username);
            HttpContext.Session.SetString("Role", account.Role);

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

        public async Task<IActionResult> Logout()
        {
            // --- SỬA TẠI ĐÂY: Dùng "AudioFlexSecurity" ---
            await HttpContext.SignOutAsync("AudioFlexSecurity");

            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            ViewData["ActivePage"] = "Login";
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            ViewData["ActivePage"] = "Login";

            if (!ModelState.IsValid) return View(model);

            if (await _accountRepository.IsUsernameExistsAsync(model.Username))
            {
                ModelState.AddModelError(nameof(model.Username), "Tên đăng nhập đã tồn tại!");
                return View(model);
            }

            var account = new Account
            {
                Username = model.Username,
                Password = model.Password,
                FullName = model.FullName,
                Email = model.Email,
                Role = "Customer"
            };

            await _accountRepository.AddAsync(account);
            await _accountRepository.SaveChangesAsync();

            TempData["RegisterSuccess"] = "Đăng ký thành công! Vui lòng đăng nhập.";
            return RedirectToAction(nameof(Login));
        }
    }
}