using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeBook.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [DisplayName("Tên người dùng")]
        public string Name { get; set; }
        [DisplayName("Ngày sinh")]
        public DateTime DoB { get; set; }
        [DisplayName("Địa chỉ")]
        public string? Address { get; set; }
        [DisplayName("Quận/Huyện")]
        public string? District { get; set; }
        [DisplayName("Tỉnh/Thành phố")]
        public string? Province { get; set; }
        [DisplayName("Ngày gia nhập")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
