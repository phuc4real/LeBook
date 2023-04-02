using AspNetCoreHero.ToastNotification.Abstractions;
using LeBook.DataAccess.Repository.IRepository;
using LeBook.Models;
using LeBook.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            if(TempData["CountErr"]!= null) 
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

            foreach (var cart in cartViewModel.ListCart)
            {
                cart.ItemTotal = cart.Book.Price.OrderByDescending(p => p.Id).FirstOrDefault().ItemPrice * cart.Count;
                if(cart.toBuy) cartViewModel.CartTotal += cart.ItemTotal;
            }

            return View(cartViewModel);
        }

        [HttpPost]
        public IActionResult CheckOut()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);


            CheckoutViewModel viewModel = new CheckoutViewModel()
            {
                Address = _unitOfWork.UserAddress.Get(claim.Value),
                ListCart = _unitOfWork.ShoppingCart.GetCart(claim.Value, true),
                Order = new()
            };

            foreach (var cart in viewModel.ListCart)
            {
                cart.ItemTotal = cart.Book.Price.OrderByDescending(p => p.Id).FirstOrDefault().ItemPrice * cart.Count;
                viewModel.CartTotal += cart.ItemTotal;
            }

            viewModel.Order.OrderTotal = viewModel.CartTotal + 30000;

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Payment(CheckoutViewModel viewModel)
        {
            if (viewModel.Order.PaymentType == "cod")
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                viewModel.Order.UserId = claim.Value;
                List<UserAddress> addresses = new List<UserAddress>();
                addresses.Add(_unitOfWork.UserAddress.GetFirtOrDefault(x => x.Id == viewModel.Order.AddressId));
                viewModel.Address = addresses;

                return View("CheckOut", viewModel);
            }
            else if (viewModel.Order.PaymentType == "vnpay")
            {
                return RedirectToAction("Index");
            }
            else return RedirectToAction("Index");
        }

        public IActionResult Plus(int cartId) 
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
            if(cart.Count <=1) 
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
                cartTotal += cart.Book.Price.OrderByDescending(p => p.Id).FirstOrDefault().ItemPrice * cart.Count;
            }

            return Json(cartTotal);
        }
        #endregion
    }
}
