using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeBook.Models.ViewModel
{
    public class BookViewModel
    {
        public Book Book {  get; set; }
        [Required]
        [DisplayName("Đơn giá")]
        [Range(1, double.MaxValue,ErrorMessage ="Giá tiền không hợp lệ!")]
        [DisplayFormat(DataFormatString = "{0:#,###.## vnđ}")]
        public double itemPrice { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CoverTypeList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> AgeList { get; set; }
}
}
