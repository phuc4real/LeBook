using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DoB { get; set; }
        [DisplayName("Ngày gia nhập")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [DisplayName("Địa chỉ giao hàng")]
        [ValidateNever]
        public virtual ICollection<UserAddress> Addresses { get; set; }
    }
}
