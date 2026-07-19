using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AudioFlex.Models
{
    /// <summary>
    /// Đơn hàng do khách hàng đặt (checkout giỏ hàng).
    /// </summary>
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        // Khách vãng lai vẫn có thể đặt hàng nên AccountId cho phép null
        [Display(Name = "Tài khoản")]
        public int? AccountId { get; set; }

        [ForeignKey(nameof(AccountId))]
        public Account Account { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên khách hàng")]
        [StringLength(150)]
        [Display(Name = "Khách hàng")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [StringLength(20)]
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng")]
        [StringLength(300)]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "Ghi chú")]
        public string Note { get; set; } // Trường này giúp khách để lại lưu ý (ví dụ: giao giờ hành chính)

        [Display(Name = "Ngày đặt")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Tổng tiền")]
        public decimal TotalAmount { get; set; }

        // Các trạng thái: "Đang xử lý", "Đang giao", "Đã giao", "Đã hủy"
        [StringLength(30)]
        [Display(Name = "Trạng thái")]
        public string Status { get; set; } = "Đang xử lý";

        // Navigation: 1 Order có nhiều OrderDetail (Chi tiết đơn hàng)
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}