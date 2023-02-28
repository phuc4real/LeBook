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
        [Required(ErrorMessage = "Tên sách được bỏ trống!")]
        public string Title { get; set; }
        [DisplayName("Mã ISBN")]
        [Required(ErrorMessage = "Mã ISBN được bỏ trống!")]
        public string ISBN { get; set; }
        [DisplayName("Mô tả nội dung")]
        public string Description { get; set; }
        [DisplayName("Hình ảnh")]
        public string ImgUrl { get; set; }
        [DisplayName("Tác giả")]
        public string Author { get; set; }
        [DisplayName("Nhà XB")]
        public string Publisher { get; set; }
        [DisplayName("Năm XB")]
        public DateTime PublicationDate { get; set; }
        [DisplayName("Tồn kho")]
        public int InStock { get; set; }
        [DisplayName("Đã bán")]
        public int Sold { get; set; }
        [Required]
        [DisplayName("Thể loại")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        [Required]
        [DisplayName("Loại bìa")]
        public int CoverTypeId { get; set; }
        [ForeignKey("CoverTypeId")]
        public CoverType CoverType { get; set; }
        [Required]
        [DisplayName("Giá bán")]
        public ICollection<Price> Price { get; set; }
    }
}
