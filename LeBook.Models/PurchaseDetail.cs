using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeBook.Models
{
    public class PurchaseDetail
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Số lượng")]
        public int Quantity { get; set; }
        [DisplayName("Giá nhập")]
        [DisplayFormat(DataFormatString = "{0:#,###.##đ}")]
        public double Price { get; set; }
        [DisplayName("Thành tiền")]
        [DisplayFormat(DataFormatString = "{0:#,###.##đ}")]
        public double Total { get; set; }

        [Required]
        [DisplayName("Mã hoá đơn")]
        public int PurchaseId { get; set; }
        [ForeignKey("PurchaseId")]
        public PurchaseOrder Purchase { get; set; }

        [Required]
        [DisplayName("Mã sách")]
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; }
    }
}
