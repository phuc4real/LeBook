using AspNetCoreHero.ToastNotification.Abstractions;
using LeBook.DataAccess.Repository.IRepository;
using LeBook.Models;
using LeBook.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeBook.Areas.Admin.Controllers
{
    //[Authorize("canView")]
    [Area("Admin")]
    public class HomeController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        public INotyfService _notifyService { get; }

        public HomeController(IUnitOfWork unitOfWork, INotyfService notifyService)
        {
            _unitOfWork = unitOfWork;
            _notifyService = notifyService;
        }
        //GET: Admin/
        public IActionResult Index()
        {
            DashboardViewModel model = new() { 
                newOrders = new(),
                sellBook = new(),
                stockBook = new(),
                SuccessOrder = 0,
                WaitingOrder = 0,
            };

            var orders = _unitOfWork.Order.Get().OrderByDescending(x => x.CreatedAt);
            var i = 1;
            foreach (var order in orders)
            {
                if (order.CreatedAt.Year == DateTime.Now.Year) model.YearRevenue += order.OrderTotal;
                if (order.CreatedAt.Month == DateTime.Now.Month) model.MonthRevenue += order.OrderTotal;
                if (order.OrderStatus == "Giao hàng thành công") model.SuccessOrder++;
                if (order.OrderStatus == "Đã đặt hàng") model.WaitingOrder++;
                if (i <= 7)
                {
                    NewOrder newOrder = new()
                    {
                        OrderId = order.Id,
                        Time = TimeElapsed(order.CreatedAt),
                    };
                    model.newOrders.Add(newOrder);
                }
                i++;
            }

            var sellbook = _unitOfWork.Book.Get(x=>x.IsDeleted == false).OrderByDescending(x=> x.Sold).Take(5);
            foreach (var book in sellbook)
            {
                BookInfo info = new()
                {
                    Id = book.Id,
                    Title = book.Title,
                    Img = book.ImgUrl,
                    Quantity =book.Sold,
                };
                model.sellBook.Add(info);
            }

            var stockbook = _unitOfWork.Book.Get(x => x.IsDeleted == false).OrderByDescending(x => x.InStock).Take(5);
            foreach (var book in stockbook)
            {
                BookInfo info = new()
                {
                    Id = book.Id,
                    Title = book.Title,
                    Img = book.ImgUrl,
                    Quantity = book.InStock,
                };
                model.stockBook.Add(info);
            }

            return View(model);
        }

        private static string TimeElapsed(DateTime date)
        {
            TimeSpan timeElapsed = DateTime.Now - date;
            int totalDays = (int)timeElapsed.TotalDays;
            int remainingHours = timeElapsed.Hours;
            int remainingMinutes = timeElapsed.Minutes;

            string formattedTime;
            if (totalDays > 0)
            {
                formattedTime = string.Format("{0} ngày", totalDays);
            }
            else if (remainingHours > 0)
            {
                formattedTime = string.Format("{0} giờ", remainingHours);
            }
            else
                formattedTime = string.Format("{0} phút", remainingMinutes);

            return formattedTime;
        }
    }
}
