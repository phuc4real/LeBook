using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeBook.Models.ViewModel
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? CreatedAt { get; set; }
        public string? OrderTotal { get; set; }
        public string? OrderStatus { get; set; }
        public string? PaymentType { get; set; }
        public string? PaymentStatus { get; set; }
        public string? Action { get; set;}
    }
}
