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
        public string PaymentType { get; set; }

        public string ShippingType { get; set; }

        public int AddressId { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,###.##đ}")]
        public double CartTotal { get; set; }

        public double OrderTotal { get; set; }

        [ValidateNever]
        public IEnumerable<UserAddress> Address { get; set; }

        [ValidateNever]
        public IEnumerable<ShoppingCart> ListCart { get; set; }
    }
}
