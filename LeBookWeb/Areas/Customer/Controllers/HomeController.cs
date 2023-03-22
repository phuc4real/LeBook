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
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ILogger<HomeController> _logger;
        public INotyfService _notifyService { get; }

        public HomeController(IUnitOfWork unitOfWork, INotyfService notifyService, ILogger<HomeController> logger)
        {
            _unitOfWork = unitOfWork;
            _notifyService = notifyService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            CHomeViewModel viewModel = new() 
            { 
                NewBook = _unitOfWork.Book.GetNewBook(),
                BestSeller = _unitOfWork.Book.Get(),
                HotDeal = _unitOfWork.Book.Get(),
                NewManga = _unitOfWork.Book.Get(),
                NewLightNovel = _unitOfWork.Book.Get(),
                TopManga = _unitOfWork.Book.Get(),
                Cate1 = _unitOfWork.Book.Get(),
                Cate2 = _unitOfWork.Book.Get(),
                Cate3 = _unitOfWork.Book.Get()
            };

            //IEnumerable<Book> books = _unitOfWork.Book.Get();

            return View(viewModel);
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
        public IActionResult Details(ShoppingCart cart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            cart.ApplicationUserId = claim.Value;

            ShoppingCart cartDB = _unitOfWork.ShoppingCart.GetFirtOrDefault(c => c.BookId ==  cart.BookId && c.ApplicationUserId == claim.Value);

            if (cartDB == null)
            {
                _unitOfWork.ShoppingCart.Add(cart);
            }
            else
            {
                _unitOfWork.ShoppingCart.IncrementCount(cartDB, cart.Count);
            }
            _unitOfWork.Save();
            _notifyService.Success("Đã thêm sản phẩm vào giỏ hàng");
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}