using AspNetCoreHero.ToastNotification.Abstractions;
using LeBook.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace LeBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public INotyfService _notifyService { get; }

        public UserController(IUnitOfWork unitOfWork,INotyfService notyfService)
        {
            _notifyService = notyfService;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var list = _unitOfWork.ApplicationUser.GetListUser();
            var userRole = _unitOfWork
            return View(list);
        }

        public IActionResult Role() { 

            return View();
        }
    }
}
