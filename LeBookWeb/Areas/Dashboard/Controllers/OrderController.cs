using AspNetCoreHero.ToastNotification.Abstractions;
using LeBook.DataAccess.Repository.IRepository;
using LeBook.Models.ViewModel;
using LeBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LeBookWeb.Areas.Admin.Controllers
{
    [Authorize("canView")]
    [Area("Dashboard")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public INotyfService _notifyService { get; }

        public OrderController(IUnitOfWork unitOfWork, INotyfService notifyService)
        {
            _unitOfWork = unitOfWork;
            _notifyService = notifyService;
        }

        // GET: Order
        public IActionResult Index()
        {
            return View( _unitOfWork.Order.Get(x=> x.OrderStatus !="Đã hoàn thành", includeProperties: "User"));
        }

        // GET: Order/Details/5
        public IActionResult Detail(int id)
        {
            var o = _unitOfWork.Order.FirstOrDefault(x => x.Id == id, includeProperties: "User");
            DetailOrderViewModel viewModel = new()
            {
                Order = o,
                ShippingAddress = _unitOfWork.UserAddress.FirstOrDefault(x=>x.Id==o.AddressId),
                orderDetails = _unitOfWork.OrderDetail.Get(x => x.OrderId == id, includeProperties: "Book"),
            };

            return View(viewModel);
        }


        // GET: Order/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, IFormCollection collection)
        {
            var order = _unitOfWork.Order.FirstOrDefault(x => x.Id == id);
            order.OrderStatus = collection["OrderStatus"];
            order.LastUpdatedAt = DateTime.Now;

            _unitOfWork.Order.Update(order);
            _unitOfWork.Save();

            _notifyService.Success("Cập nhật trạng thái đơn hàng thành công");
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelOrder(int id)
        {

            var order = _unitOfWork.Order.FirstOrDefault(x => x.Id == id);
            order.OrderStatus = "Đã bị huỷ";
            order.LastUpdatedAt = DateTime.Now;
            var orderdetails = _unitOfWork.OrderDetail.Get(x=>x.OrderId == id, includeProperties: "Book");

            foreach (var item in orderdetails)
            {
                _unitOfWork.Book.UpdateBookQuantity(item.Book, item.Quantity,true);
            }

            _unitOfWork.Order.Update(order);
            _unitOfWork.Save();

            return RedirectToAction("Index");
        }

        public IActionResult Completed()
        {
            return View(_unitOfWork.Order.Get(x => x.OrderStatus == "Đã hoàn thành", includeProperties: "User"));
        }

        #region API 
        [HttpGet]
        public IActionResult YearRevenue(int Year)
        {
            var orders = _unitOfWork.Order.Get(x => x.CreatedAt.Year == Year);
            var listRevenue = Enumerable.Range(1, 12)
                .Select(i => new ItemModel { Name = " Tháng " +i}).ToList();

            var ordersByMonth = orders.GroupBy(x => x.CreatedAt.Month);
            foreach (var monthGroup in ordersByMonth)
            {
                var monthNumber = monthGroup.Key;
                var monthRevenue = monthGroup.Sum(x => x.OrderTotal);
                listRevenue[monthNumber - 1].Value = monthRevenue;
            }
            return Ok(listRevenue);
        }

        [HttpGet]
        public IActionResult MonthRevenue(int Year, int Month)
        {
            var orders = _unitOfWork.Order.Get(x=>x.CreatedAt.Year == Year && x.CreatedAt.Month == Month);
            var listRevenue = Enumerable.Range(1, DateTime.DaysInMonth(Year, Month))
                .Select(i => new ItemModel { Name = "Ngày " + i }).ToList();

            var ordersByDayOfMonth = orders.GroupBy(x => x.CreatedAt.Day );
            foreach (var dayGroup in ordersByDayOfMonth)
            {
                var dayNumber = dayGroup.Key;
                var dayRevenue = dayGroup.Sum(x => x.OrderTotal);
                listRevenue[dayNumber - 1].Value = dayRevenue;
            }

            return Ok(listRevenue);
        }

        [HttpGet]
        public IActionResult GetBookSales(int id)
        {
            List<ItemModel> list = new();

            DateTime last30Days = DateTime.Today.AddDays(-30);

            for (DateTime i = last30Days; i < DateTime.Today; i=i.AddDays(1)) {
                list.Add(new ItemModel() { Name = "Ngày " + i.Day + "-" + i.Month });
            }

            var orderdetals = _unitOfWork.OrderDetail.Get(x=> x.BookId == id && x.Order.CreatedAt > last30Days, includeProperties:"Order");

            var salesInLast30Days = orderdetals.GroupBy(x => new {x.Order.CreatedAt.Day , x.Order.CreatedAt.Month});

            foreach (var sale in salesInLast30Days)
            {
                var index = list.FindIndex(0,x=> x.Name == "Ngày " + sale.Key.Day + "-" + sale.Key.Month);
                list[index].Value = sale.Sum(x => x.Quantity);
            }

            return Ok(list);
        }

        public IActionResult OrderList()
        {
            var orders = _unitOfWork.Order.Get(x => x.OrderStatus == "Đã đặt hàng", includeProperties: "User").OrderByDescending(x=> x.CreatedAt);

            List<OrderViewModel> models = new List<OrderViewModel>();
            foreach (var order in orders)
            {
                OrderViewModel item = new()
                {
                    Id = order.Id,
                    Username = order.User.Name,
                    CreatedAt = order.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss"),
                    OrderTotal = order.OrderTotal.ToString("#,###.##đ"),
                    OrderStatus = order.OrderStatus,
                    PaymentType = order.PaymentType,
                    PaymentStatus = order.PaymentStatus,
                    Action = "<a href=\"/Dashboard/Order/Detail/" + order.Id + "\">Chi tiết hoá đơn</a>",
                };

                    //if (User.IsInRole(Roles.Admin) || User.IsInRole(Roles.Manager)) {
                    //item.Action += "| <a href=\"/Dashboard/Order/Edit/" + order.Id + "\">Cập nhật</a>\r\n| " +
                    //    "<a href=\"#\" onclick=\"return submitDelete(event,'"+order.Id+"','Đơn đặt hàng')\">Xoá</a>\r\n" +
                    //    "<form id=\"Delete+" + order.Id + "\" action=\"/Dashboard/Order/Delete\" method=\"post\">\r\n" +
                    //    "<input type=\"hidden\" value=\"" + order.Id + "\" name=\"id\" id=\"id\"></form>";
                    //}

                models.Add(item);
            }
            return Ok(models);
        }

        #endregion
    }
}
