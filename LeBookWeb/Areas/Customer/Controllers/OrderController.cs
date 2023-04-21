using LeBook.DataAccess.Repository.IRepository;
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
