using System.ComponentModel.DataAnnotations;

namespace AudioFlex.Models
{
    /// <summary>
    /// Tin tức / bài viết hiển thị ở trang Blog (blog.html).
    /// </summary>
    public class Blog
    {
        [Key]
        public int BlogId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
        [StringLength(200)]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập nội dung")]
        [Display(Name = "Nội dung")]
        public string Content { get; set; }

        [StringLength(300)]
        [Display(Name = "Hình ảnh")]
        public string ImagePath { get; set; }

        [Display(Name = "Ngày đăng")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
