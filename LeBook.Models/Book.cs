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
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Tên sách")]
        [Required(ErrorMessage = "Tên sách không được bỏ trống!")]
        public string Title { get; set; }
        [DisplayName("ISBN")]
        [Required(ErrorMessage = "ISBN không được bỏ trống!")]
        public string ISBN { get; set; }
        [DisplayName("Mô tả nội dung")]
        [Required(ErrorMessage = "Mô tả nội dung không được bỏ trống!")]
        public string Description { get; set; }
        [DisplayName("Hình ảnh")]
        [ValidateNever]
        public string ImgUrl { get; set; }
        [DisplayName("Tác giả")]
        [Required(ErrorMessage = "Tác giả không được bỏ trống!")]
        public string Author { get; set; }
        [DisplayName("Nhà XB")]
        [Required(ErrorMessage = "Nhà XB không được bỏ trống!")]
        public string Publisher { get; set; }
        [DisplayName("Năm XB")]
        [Required(ErrorMessage = "Năm XB không được bỏ trống!")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime PublicationDate { get; set; }
        [DisplayName("Tồn kho")]
        [Required(ErrorMessage = "Tồn kho không được bỏ trống!")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng không hợp lệ")]
        public int InStock { get; set; }
        [DisplayName("Đã bán")]
        public int Sold { get; set; }
        [DisplayName("Thể loại")]
        [Required(ErrorMessage = "Thể loại chưa được chọn!")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }
        [DisplayName("Loại bìa")]
        [Required(ErrorMessage = "Loại bìa chưa được chọn!")]
        public int CoverTypeId { get; set; }
        [ForeignKey("CoverTypeId")]
        [ValidateNever]
        public CoverType CoverType { get; set; }
        [DisplayName("Phân loại độ tuổi")]
        [Required(ErrorMessage = "Phân loại độ tuổi chưa được chọn!")]
        public int AgeId { get; set; }
        [ForeignKey("AgeId")]
        [ValidateNever]
        public Age Age { get; set; }
        [DisplayName("Giá bán")]
        [ValidateNever]
        public virtual ICollection<Price> Price { get; set; }
        [DisplayName("Ngày tạo")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [DisplayName("Ngày chỉnh sửa")]
        public DateTime LastUpdatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        [DisplayName("Ngày xoá")]
        public DateTime DeletedAt { get; set; }
    }
}
