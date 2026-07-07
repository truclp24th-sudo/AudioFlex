using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SpicyKorea.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller cha cho toàn bộ khu vực Admin.
    /// Mọi Controller trong Areas/Admin đều kế thừa từ đây để tự động
    /// kiểm tra Session: chưa đăng nhập hoặc không phải Role "Admin" -> chuyển về trang Đăng nhập.
    /// </summary>
    [Area("Admin")]
    public abstract class AdminBaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(role) || role != "Admin")
            {
                // Chưa đăng nhập hoặc không có quyền Admin -> chuyển hướng về trang đăng nhập
                context.Result = new RedirectToActionResult("Login", "Account", new { area = "", returnUrl = context.HttpContext.Request.Path });
                return;
            }

            // Đưa tên đăng nhập ra layout để hiển thị trên header
            ViewBag.CurrentUsername = HttpContext.Session.GetString("Username");

            base.OnActionExecuting(context);
        }
    }
}
