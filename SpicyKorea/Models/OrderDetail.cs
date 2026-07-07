using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpicyKorea.Models
{
    /// <summary>
    /// Chi tiết đơn hàng: từng sản phẩm trong 1 Order.
    /// </summary>
    public class OrderDetail
    {
        [Key]
        public int OrderDetailId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

        // Lưu lại tên sản phẩm tại thời điểm đặt hàng (phòng khi sản phẩm bị đổi tên/xóa)
        [StringLength(200)]
        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; }

        [Required]
        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }

        // Đơn giá tại thời điểm đặt hàng
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Đơn giá")]
        public decimal Price { get; set; }
    }
}
