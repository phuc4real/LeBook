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
            ViewBag.CountErr = TempData["CountErr"] as string;
            TempData.Remove("CountErr");

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            cartViewModel = new CartViewModel()
            {
                ListCart = _unitOfWork.ShoppingCart.GetCart(claim.Value)
            };

            foreach (var cart in cartViewModel.ListCart)
            {
                cart.ItemTotal = cart.Book.Price.OrderByDescending(p => p.Id).FirstOrDefault().ItemPrice * cart.Count;
                cartViewModel.CartTotal += cart.ItemTotal;
            }

            return View(cartViewModel);
        }

        public IActionResult CheckOut()
        {
            //ViewBag.CountErr = TempData["CountErr"] as string;
            //TempData.Remove("CountErr");

            //var claimsIdentity = (ClaimsIdentity)User.Identity;
            //var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //cartViewModel = new CartViewModel()
            //{
            //    ListCart = _unitOfWork.ShoppingCart.GetCart(claim.Value)
            //};

            //foreach (var cart in cartViewModel.ListCart)
            //{
            //    cart.ItemTotal = cart.Book.Price.OrderByDescending(p => p.Id).FirstOrDefault().ItemPrice * cart.Count;
            //    cartViewModel.CartTotal += cart.ItemTotal;
            //}

            return View();
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
    }
}
