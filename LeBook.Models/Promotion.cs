using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeBook.Models
{
    public class Promotion
    {
        [Key]
        [DisplayName("Mã khuyến mãi")]
        public int Id { get; set; }
        [DisplayName("Tên khuyến mãi")]
        public string Name { get; set; }
        [DisplayName("Banner khuyến mãi")]
        public string PromotionBanner { get; set; }
        [DisplayName("Thời gian bắt đầu")]
        public DateTime StartedDate { get; set; }
        [DisplayName("Thời gian kết thúc")]
        public DateTime EndedDate { get; set; }
        [DisplayName("Tỉ lệ khuyến mãi")]
        public double Percent { get; set; }
        [DisplayName("Mô tả khuyến mãi")]
        public string Description { get; set; }

    }
}
