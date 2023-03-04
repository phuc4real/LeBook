using AspNetCoreHero.ToastNotification.Abstractions;
using LeBook.DataAccess.Repository.IRepository;
using LeBook.Models;
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
            IEnumerable<Book> books = _unitOfWork.Book.Get();

            return View(books);
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

           _unitOfWork.ShoppingCart.Add(cart);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}