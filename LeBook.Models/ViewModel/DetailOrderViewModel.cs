using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LeBook.Models.ViewModel
{
    public class DetailOrderViewModel
    {
        public Order Order { get; set; }

        [JsonIgnore]
        public UserAddress ShippingAddress { get; set; }

        public IEnumerable<OrderDetail> orderDetails { get; set; }
    }
}
