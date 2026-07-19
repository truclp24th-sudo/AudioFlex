using System.ComponentModel.DataAnnotations;

namespace AudioFlex.Models
{
    public class ProductImage
    {
        [Key]
        public int ProductImageId { get; set; }

        [Required]
        public string ImagePath { get; set; } // Lưu tên file ảnh (vd: th11-be.jpg)

        public string? ColorName { get; set; } // Lưu tên màu nếu cần (vd: Trắng, Đen)

        // Khóa ngoại liên kết tới bảng Product
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}