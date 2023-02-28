using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LeBook.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên thể loại được bỏ trống!")]
        [DisplayName("Thể loại")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "STT được bỏ trống!")]
        [DisplayName("Số thứ tự")]
        [Range(0, int.MaxValue, ErrorMessage ="STT không hợp lệ")]
        public int DisplayOrder { get; set; }
        [DisplayName("Ngày tạo")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [DisplayName("Ngày chỉnh sửa")]
        public DateTime LastUpdatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        [DisplayName("Ngày xoá")]
        public DateTime DeletedAt { get; set; }
    }
}