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
        public string? PromotionBanner { get; set; }
        [DisplayName("Thời gian bắt đầu")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime StartedDate { get; set; }
        [DisplayName("Thời gian kết thúc")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime EndedDate { get; set; }
        [DisplayName("Tỉ lệ khuyến mãi")]
        public double Percent { get; set; }
        [DisplayName("Mô tả khuyến mãi")]
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastUpdatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }
        public DateTime DeletedAt { get; set; }

    }
}
