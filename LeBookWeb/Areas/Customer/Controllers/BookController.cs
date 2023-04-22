using AspNetCoreHero.ToastNotification.Abstractions;
using LeBook.DataAccess.Repository.IRepository;
using LeBook.Models;
using LeBook.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace LeBook.Controllers
{
    [Area("Customer")]
    public class BookController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public INotyfService _notifyService { get; }

        public BookController(IUnitOfWork unitOfWork, INotyfService notifyService)
        {
            _unitOfWork = unitOfWork;
            _notifyService = notifyService;
        }


        public IActionResult Search(string search)
        {
            return View();
        }

        public IActionResult Details(int bookId)
        {
            ShoppingCart cart = new()
            {
                Book = _unitOfWork.Book.GetFirst(bookId),
                BookId= bookId,
                Count = 1
            };
           
            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public  IActionResult Details(ShoppingCart cart,string? returnurl)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            cart.ApplicationUserId = claim.Value;

            ShoppingCart cartDB = _unitOfWork.ShoppingCart.FirstOrDefault(c => c.BookId ==  cart.BookId && c.ApplicationUserId == claim.Value);

            if (cartDB == null)
            {
                _unitOfWork.ShoppingCart.Add(cart);
            }
            else
            {
                _unitOfWork.ShoppingCart.IncrementCount(cartDB, cart.Count);
            }
            _unitOfWork.Save();

            if (returnurl != null) {
                _notifyService.Success("Đã thêm sản phẩm vào giỏ hàng");
                return RedirectToAction("Index", "Home");
            }
            else return RedirectToAction("Index", "Cart");
        }
    }
}