using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeBook.Models.ViewModel
{
    public class PurchaseViewModel
    {
        public PurchaseOrder PurchaseOrder { get; set; }
        public IEnumerable<SelectListItem> Company { get; set; }
        public IEnumerable<PurchaseDetail> PurchaseDetails { get; set; }
    }

    public class PurchaseItem
    {
        public int Quantity { get; set; }
        public int BookId { get; set; }
        public double Price { get; set; }
    }
}
