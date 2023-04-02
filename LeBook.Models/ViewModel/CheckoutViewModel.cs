using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeBook.Models.ViewModel
{
    public class CheckoutViewModel
    {
        [DisplayFormat(DataFormatString = "{0:#,###.##đ}")]
        public double CartTotal { get; set; }

        public IEnumerable<UserAddress> Address { get; set; }

        public IEnumerable<ShoppingCart> ListCart { get; set; }

        public Order Order { get; set; }
    }
}
