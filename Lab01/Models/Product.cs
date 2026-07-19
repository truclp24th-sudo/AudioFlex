using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AudioFlex.Models
{
    /// <summary>
    /// Sản phẩm (mì cay, đồ uống, ...) hiển thị ngoài trang người dùng và quản lý trong Admin.
    /// </summary>
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        [StringLength(200)]
        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Giá")]
        public decimal Price { get; set; }

        // Giá gốc (trước khuyến mãi) - có thể null nếu không giảm giá
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Giá gốc")]
        public decimal? OldPrice { get; set; }

        [StringLength(300)]
        [Display(Name = "Hình ảnh")]
        public string ImagePath { get; set; }

        [StringLength(1000)]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Sản phẩm mới")]
        public bool IsNew { get; set; } = true;

        [Display(Name = "Ngày tạo")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Khóa ngoại tới Category
        [Required]
        [Display(Name = "Danh mục")]
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }

        // Navigation: 1 Product - N OrderDetail
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
    }
}
