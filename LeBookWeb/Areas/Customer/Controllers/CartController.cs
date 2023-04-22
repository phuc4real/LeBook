using AspNetCoreHero.ToastNotification.Abstractions;
using LeBook.DataAccess.Repository.IRepository;
using LeBook.Models;
using LeBook.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace LeBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public INotyfService _notifyService { get; }
        public CartViewModel cartViewModel { get; set; }
        public double OrderTotal { get; set; }

        public CartController(IUnitOfWork unitOfWork, INotyfService notifyService)
        {
            _unitOfWork = unitOfWork;
            _notifyService = notifyService;
        }

        public IActionResult Index()
        {
            if (TempData["CountErr"] != null)
            {
                ViewBag.CountErr = TempData["CountErr"] as string;
                TempData.Remove("CountErr");
            }

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            cartViewModel = new CartViewModel()
            {
                ListCart = _unitOfWork.ShoppingCart.GetCart(claim.Value, false),
                CartTotal = 0
            };

            List<Promotion> ListPromo = new List<Promotion>();

            foreach (var cart in cartViewModel.ListCart)
            {
                cart.ItemTotal = cart.Book.Price.OrderByDescending(p => p.Id).FirstOrDefault().ItemPrice * cart.Count;
                if (cart.toBuy) cartViewModel.CartTotal += cart.ItemTotal;
                var PromoDetail = _unitOfWork.PromotionDetail.Get(x => x.BookId == cart.BookId, includeProperties: "Promotion");
                foreach (var item in PromoDetail)
                {
                    ListPromo.Add(item.Promotion);
                }
            }
            cartViewModel.Promotions = ListPromo;

            return View(cartViewModel);
        }

        [HttpPost]
        public IActionResult CheckOut()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var a = _unitOfWork.UserAddress.GetAdress(claim.Value);

            CheckoutViewModel viewModel = new CheckoutViewModel()
            {
                Address = _unitOfWork.UserAddress.GetAdress(claim.Value),
                ListCart = _unitOfWork.ShoppingCart.GetCart(claim.Value, true),
                Order = new(),
            };

            foreach (var cart in viewModel.ListCart)
            {
                cart.Book.Price.OrderByDescending(p => p.Id).First().ItemPrice = GetPrice(cart.BookId);
                cart.ItemTotal = cart.Book.Price.OrderByDescending(p => p.Id).First().ItemPrice * cart.Count;

                //var PromoDetail = _unitOfWork.PromotionDetail.Get(x => x.BookId == cart.BookId, includeProperties: "Promotion");
                //foreach (var item in PromoDetail)
                //{
                //    cart.Book.Price.OrderByDescending(p => p.Id).First().ItemPrice *= item.Promotion.Percent / 100;
                //    cart.ItemTotal = cart.Book.Price.OrderByDescending(p => p.Id).First().ItemPrice * cart.Count;
                //}
                viewModel.CartTotal += cart.ItemTotal;

            }
            viewModel.Order.OrderTotal = viewModel.CartTotal + 30000;

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Payment(CheckoutViewModel viewModel)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            Order order = viewModel.Order;
            order.OrderTotal = order.OrderTotal + 30000;
            order.UserId = claim.Value;
            order.ShippingDate = DateTime.Now.AddDays(7);
            order.OrderStatus = "Đã đặt hàng";
            order.PaymentStatus = "Chưa thanh toán";

            _unitOfWork.Order.Add(order);

            IEnumerable<ShoppingCart> carts = _unitOfWork.ShoppingCart.GetCart(claim.Value, true);

            foreach (var cart in carts) {

                OrderDetail detail = new()
                {
                    Quantity = cart.Count,
                    Order = order,
                    Book = cart.Book,
                    Price = GetPrice(cart.BookId),
                    Total = GetPrice(cart.BookId) * cart.Count,

                };
                _unitOfWork.Book.UpdateBookQuantity(cart.Book, cart.Count);
                _unitOfWork.ShoppingCart.Remove(cart);
                _unitOfWork.OrderDetail.Add(detail);
            }

            _unitOfWork.Save();

            if (order.PaymentType == "cod")
            {
                return RedirectToAction("Order", new { orderId = order.Id });
            }
            else if (order.PaymentType == "vnpay")
            {
                return RedirectToAction("Order", new { orderId = order.Id });
            }
            else return RedirectToAction("Index");
        }

        public IActionResult Order(int orderId)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            Order order = _unitOfWork.Order.FirstOrDefault(x=> x.Id==orderId);

            if (order.UserId == claim.Value) 
            {
                List<UserAddress> addresses = new()
                {
                    _unitOfWork.UserAddress.FirstOrDefault(x => x.Id == order.AddressId)
                };

                CheckoutViewModel viewModel = new CheckoutViewModel()
                {
                    Address = addresses,
                    OrderDetail = _unitOfWork.OrderDetail.Get(x=>x.OrderId == orderId, includeProperties: "Book"),
                    Order = order
                };

                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public  IActionResult Plus(int cartId) 
        {
            var cart = _unitOfWork.ShoppingCart.GetCartById(cartId);
            if (cart.Count >= 10)
                TempData["CountErr"] = "Số lượng mỗi sản phẩm tối đa là 10!";
            else
            {
                _unitOfWork.ShoppingCart.IncrementCount(cart, 1);
                _unitOfWork.Save();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetCartById(cartId);
            if (cart.Count <=1) 
                _unitOfWork.ShoppingCart.Remove(cart);
            else 
                _unitOfWork.ShoppingCart.DecrementCount(cart, 1);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetCartById(cartId);
            _unitOfWork.ShoppingCart.Remove(cart);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }


        public double GetPrice(int bookid)
        {

            double res = 0;
            var PromoDetails = _unitOfWork.PromotionDetail.Get(x => x.BookId == bookid, includeProperties: "Promotion");
            var PromoDetail = PromoDetails.OrderByDescending(x=>x.Promotion.Percent).FirstOrDefault();

            var book = _unitOfWork.Book.GetFirst(bookid);
            if (PromoDetail != null)
            {
                res = book.Price.OrderByDescending(p => p.Id).First().ItemPrice * PromoDetail.Promotion.Percent / 100;
            }
            else
            {
                res = book.Price.OrderByDescending(p => p.Id).First().ItemPrice;
            }
            return res;
        }



        #region API

        [HttpPost]
        public IActionResult ToBuy(string cartId)
        {
            bool res = _unitOfWork.ShoppingCart.ToBuy(int.Parse(cartId));
            _unitOfWork.Save();
            return Json(res);
        }

        public IActionResult TotalPrice()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            double cartTotal = 0;
            IEnumerable<ShoppingCart> ListCart = _unitOfWork.ShoppingCart.GetCart(claim.Value, true);

            foreach (var cart in ListCart)
            {
                cartTotal += cart.Book.Price.OrderByDescending(p => p.Id).First().ItemPrice * cart.Count;
            }

            return Json(cartTotal);
        }

        [HttpGet]
        public IActionResult SaleOff()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var ListCart = _unitOfWork.ShoppingCart.GetCart(claim.Value, false);

            double CartTotal = 0;
            double Sale = 0;

            foreach (var cart in ListCart)
            {
                cart.ItemTotal = GetPrice(cart.BookId) * cart.Count;
                Sale += ((cart.Book.Price.OrderByDescending(p => p.Id).First().ItemPrice * cart.Count) - cart.ItemTotal);
                CartTotal += cart.ItemTotal;

            }

            return Ok(new { Sale, CartTotal }) ;
        }
        #endregion
    }
}
