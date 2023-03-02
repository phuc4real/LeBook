using AspNetCoreHero.ToastNotification.Abstractions;
using LeBook.DataAccess.Repository.IRepository;
using LeBook.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LeBook.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public INotyfService _notifyService { get; }

        public HomeController(IUnitOfWork unitOfWork, INotyfService notifyService)
        {
            _unitOfWork = unitOfWork;
            _notifyService = notifyService;
        }

        public IActionResult Index()
        {
            IEnumerable<Book> books = _unitOfWork.Book.Get();

            return View(books);
        }

        public IActionResult Details(int id)
        {
            Book book = _unitOfWork.Book.GetFirst(id);
            return View();
        }
    }
}