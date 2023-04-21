using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeBook.Models.ViewModel
{
    public class DashboardViewModel
    {
        public List<NewOrder> newOrders { get; set; }
        public List<BookInfo> sellBook { get; set; }
        public List<BookInfo> stockBook { get; set; }
        public double MonthRevenue { get; set; }
        public double YearRevenue { get;set; }
        public int SuccessOrder { get; set; }
        public int WaitingOrder { get; set; }
    }

    public class NewOrder
    {
        public int OrderId { get; set; }
        public string OrderStatus { get; set; }
        public string Time { get; set; }
    }

    public class BookInfo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Img { get; set; }
        public int Quantity { get; set; }
    }
}
