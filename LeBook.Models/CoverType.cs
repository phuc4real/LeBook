using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LeBook.Models
{
    public class CoverType
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên loại bìa không được bỏ trống!")]
        [DisplayName("Loại bìa")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "STT không được bỏ trống!")]
        [Range(0, int.MaxValue, ErrorMessage = "STT không hợp lệ")]
        [DisplayName("Số thứ tự")]
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
