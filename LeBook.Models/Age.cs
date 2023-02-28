using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LeBook.Models
{
    public class Age
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Độ tuổi")]
        public string? Name { get; set; }
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
