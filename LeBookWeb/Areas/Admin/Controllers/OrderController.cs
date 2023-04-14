using AspNetCoreHero.ToastNotification.Abstractions;
using LeBook.DataAccess.Repository.IRepository;
using LeBook.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LeBookWeb.Areas.Admin.Controllers
{
    [Authorize("canView")]
    [Area("Admin")]
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
            return View( _unitOfWork.Order.Get(x=> x.OrderStatus !="Đã xác nhận"));
        }

        // GET: Order/Details/5
        public ActionResult Details(int id)
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

        #endregion
    }
}
