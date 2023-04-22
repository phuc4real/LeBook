using LeBook.DataAccess.Repository.IRepository;
using LeBook.Models;
using LeBook.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LeBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(int id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            Order order = _unitOfWork.Order.FirstOrDefault(x => x.Id == id);

            if (order.UserId == claim.Value)
            {
                List<UserAddress> addresses = new()
                {
                    _unitOfWork.UserAddress.FirstOrDefault(x => x.Id == order.AddressId)
                };

                CheckoutViewModel viewModel = new CheckoutViewModel()
                {
                    Address = addresses,
                    OrderDetail = _unitOfWork.OrderDetail.Get(x => x.OrderId == id, includeProperties: "Book"),
                    Order = order
                };

                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        #region API

        [HttpGet]
        public IActionResult OrderList()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var orders = _unitOfWork.Order.Get(x => x.OrderStatus == "Đã đặt hàng" && x.UserId == claim.Value, includeProperties: "User").OrderByDescending(x => x.CreatedAt);

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
                    Action = "<a class=\"text-decoration-none\" href=\"/Customer/Order/Detail/" + order.Id + "\">Chi tiết hoá đơn</a>",
                };

                models.Add(item);
            }
            return Ok(models);
        }

        #endregion
    }
}
