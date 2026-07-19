using System.ComponentModel.DataAnnotations;

namespace AudioFlex.Models
{
    /// <summary>
    /// Danh mục sản phẩm (VD: Mỳ Cay, Đồ Uống, Best Seller).
    /// </summary>
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên danh mục")]
        [StringLength(100)]
        [Display(Name = "Tên danh mục")]
        public string CategoryName { get; set; }

        [StringLength(250)]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        // Navigation property: 1 Category - N Product
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
