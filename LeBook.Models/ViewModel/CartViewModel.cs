using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeBook.Models.ViewModel
{
    public class CartViewModel
    {
        public IEnumerable<ShoppingCart> ListCart { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,###.##đ}")]
        public double CartTotal { get; set; }
    }
}
