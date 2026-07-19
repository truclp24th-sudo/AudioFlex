using System.ComponentModel.DataAnnotations;

namespace AudioFlex.Models
{
    /// <summary>
    /// Tài khoản đăng nhập dùng chung cho Admin và khách hàng.
    /// LƯU Ý (theo yêu cầu đồ án): mật khẩu lưu dạng PLAIN TEXT, KHÔNG mã hóa/hash,
    /// đăng nhập chỉ so sánh trực tiếp Username/Password. Không dùng ASP.NET Identity.
    /// </summary>
    public class Account
    {
        [Key]
        public int AccountId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        [StringLength(100)]
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }

        // Plain text - KHÔNG hash, KHÔNG mã hóa (yêu cầu đồ án môn học)
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [StringLength(100)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        // "Admin" hoặc "Customer"
        [Required]
        [StringLength(20)]
        [Display(Name = "Vai trò")]
        public string Role { get; set; } = "Customer";

        [StringLength(150)]
        [Display(Name = "Họ tên")]
        public string FullName { get; set; }

        [StringLength(150)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation: 1 Account - N Order
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
