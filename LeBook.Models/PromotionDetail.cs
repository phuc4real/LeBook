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
    public class PromotionDetail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Mã khuyến mãi")]
        public int PromotionId { get; set; }
        [ForeignKey("PromotionId")]
        public Promotion Promotion { get; set; }

        [Required]
        [DisplayName("Mã sách")]
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; }
    }
}
