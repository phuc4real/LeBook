using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeBook.Models.ViewModel
{
    public class ShoppingCart
    {
        public Book book { get; set; }
        [Range(0, 5, ErrorMessage ="Mua tối đa 5 sản phẩm")]
        public int Count { get; set; }
    }
}
