using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeBook.Models
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }
        
        [DisplayName("Số lượng")]
        public int Quantity { get; set; }

        [DisplayName("Đơn giá")]
        [DisplayFormat(DataFormatString = "{0:#,###.##đ}")]
        public double Price { get; set; }

        [DisplayName("Thành tiền")]
        [DisplayFormat(DataFormatString = "{0:#,###.##đ}")]
        public double Total { get; set; }

        [Required]
        [DisplayName("Mã hoá đơn")]
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        [ValidateNever]
        public Order Order { get; set; }

        [Required]
        [DisplayName("Mã sách")]
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        [ValidateNever]
        public Book Book { get; set; }
    }
}
