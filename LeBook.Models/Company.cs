using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeBook.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Tên nhà cung cấp")]
        public string Name { get; set; }
        [DisplayName("Địa chỉ")]
        public string? Address { get; set; }
        [DisplayName("Xã/Phường")]
        public string? Ward { get; set; }
        [DisplayName("Quận/Huyện")]
        public string? District { get; set; }
        [DisplayName("Tỉnh/Thành phố")]
        public string? Province { get; set; }
        [DisplayName("Số điện thoại")]
        public string? PhoneNumber { get; set; }
        [DisplayName("Địa chỉ mail")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "STT không được bỏ trống!")]
        [DisplayName("Số thứ tự")]
        [Range(0, int.MaxValue, ErrorMessage = "STT không hợp lệ")]
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
