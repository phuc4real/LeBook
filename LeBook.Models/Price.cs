using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeBook.Models
{
    public class Price
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Giá tiền")]
        [Range(1, Double.MaxValue,ErrorMessage ="Giá tiền không hợp lệ")]
        public double ItemPrice { get; set; }
        [Required]
        [DisplayName("Ngày áp dụng")]
        public DateTime Date { get; set; }
        [DisplayName("Ngày tạo")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [DisplayName("Ngày chỉnh sửa")]
        public DateTime LastUpdatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        [DisplayName("Ngày xoá")]
        public DateTime DeletedAt { get; set; }
        [Required]
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; }
    }
}
