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
            return View( _unitOfWork.Order.Get(x=> x.OrderStatus !="Đã xác nhận", includeProperties: "User"));
        }

        // GET: Order/Details/5
        public ActionResult Detail(int id)
        {
            return View();
        }

        // GET: Order/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Order/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Order/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Order/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        #region API 
        [HttpGet]
        public IActionResult YearRevenue(int Year)
        {
            var orders = _unitOfWork.Order.Get(x => x.CreatedAt.Year == Year);
            var listRevenue = Enumerable.Range(1, 12)
                .Select(i => new Revenue { Name = " Tháng " +i}).ToList();

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
                .Select(i => new Revenue { Name = "Ngày " + i }).ToList();

            var ordersByDayOfMonth = orders.GroupBy(x => x.CreatedAt.Day );
            foreach (var dayGroup in ordersByDayOfMonth)
            {
                var dayNumber = dayGroup.Key;
                var dayRevenue = dayGroup.Sum(x => x.OrderTotal);
                listRevenue[dayNumber - 1].Value = dayRevenue;
            }

            return Ok(listRevenue);
        }

        public IActionResult OrderList()
        {
            var orders = _unitOfWork.Order.Get(x => x.OrderStatus != "Đã xác nhận", includeProperties: "User").OrderByDescending(x=> x.CreatedAt);

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
