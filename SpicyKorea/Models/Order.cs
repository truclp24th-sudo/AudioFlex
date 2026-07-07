using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpicyKorea.Models
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

        [StringLength(20)]
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        [StringLength(300)]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "Ngày đặt")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Tổng tiền")]
        public decimal TotalAmount { get; set; }

        // "Đang xử lý", "Đang giao", "Đã giao"
        [StringLength(30)]
        [Display(Name = "Trạng thái")]
        public string Status { get; set; } = "Đang xử lý";

        // Navigation: 1 Order - N OrderDetail
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
