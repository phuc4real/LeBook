using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LeBook.Models
{
    public class PurchaseOrder
    {
        [Key]
        [DisplayName("Mã đơn nhập")]
        public int Id { get; set; }
        [DisplayName("Ngày lập")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [DisplayName("Tổng chi")]
        [DisplayFormat(DataFormatString = "{0:#,###.##đ}")]
        public double PurchaseTotal { get; set; }



        [DisplayName("Nhà cung cấp")]
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public Company? Company { get; set; }

        [DisplayName("Người nhập hàng")]
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
    }
}
