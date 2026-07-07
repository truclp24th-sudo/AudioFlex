using System.ComponentModel.DataAnnotations;

namespace SpicyKorea.ViewModels
{
    /// <summary>
    /// Dùng cho form đăng nhập (dùng chung cho Admin và Customer).
    /// </summary>
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
