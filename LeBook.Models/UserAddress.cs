using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LeBook.Models
{
    public class UserAddress
    {
        public int Id { get; set; }

        [DisplayName("Tên người nhận")]
        public string? Name { get; set; }
        [DisplayName("Số điện thoại giao hàng")]
        public string Contact { get; set; }
        [DisplayName("Địa chỉ")]
        public string? Street { get; set; }
        [DisplayName("Xã/Phường")]
        public string? Ward { get; set; }
        [DisplayName("Quận/Huyện")]
        public string? District { get; set; }
        [DisplayName("Tỉnh/Thành phố")]
        public string? Province { get; set; }
        [DisplayName("Ngày tạo")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [DisplayName("Ngày chỉnh sửa")]
        public DateTime LastUpdatedAt { get; set; } = DateTime.Now;

        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        [JsonIgnore]
        public ApplicationUser User { get; set; }
    }
}
