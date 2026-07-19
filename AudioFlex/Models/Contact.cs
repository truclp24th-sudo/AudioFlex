using System.ComponentModel.DataAnnotations;

namespace AudioFlex.Models
{
    /// <summary>
    /// Thông tin liên hệ do khách hàng gửi từ trang contact.html.
    /// Admin xem lại tại Areas/Admin (trang Feedback).
    /// </summary>
    public class Contact
    {
        [Key]
        public int ContactId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [StringLength(150)]
        [Display(Name = "Họ tên")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [StringLength(150)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(200)]
        [Display(Name = "Tiêu đề")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập nội dung")]
        [Display(Name = "Nội dung")]
        public string Message { get; set; }

        [Display(Name = "Thời gian gửi")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // --- THÊM DÒNG NÀY ĐỂ LƯU PHẢN HỒI ---
        [Display(Name = "Phản hồi khách hàng")]
        public string? AdminReply { get; set; }
    }
}
