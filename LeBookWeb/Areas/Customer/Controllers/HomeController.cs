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
        public INotyfService _notifyService { get; }

        public HomeController(IUnitOfWork unitOfWork, INotyfService notifyService)
        {
            _unitOfWork = unitOfWork;
            _notifyService = notifyService;
        }

        public IActionResult Index()
        {
            CHomeViewModel viewModel = new() 
            { 
                Promotion = _unitOfWork.Promotion.Get(x=>x.IsDeleted == false).OrderByDescending(x=>x.CreatedAt).Take(2),
                PromotionCarousel = _unitOfWork.Promotion.Get(x => x.IsDeleted == false).OrderByDescending(x => x.CreatedAt).Take(5),
                NewBook = _unitOfWork.Book.Get10("new"),
                BestSeller = _unitOfWork.Book.Get10("bestseller"),
                HotDeal = _unitOfWork.Book.Get10("hotdeal"),
                NewManga = _unitOfWork.Book.Get10("newmanga"),
                NewLightNovel = _unitOfWork.Book.Get10("newlightnovel"),
                TopManga = _unitOfWork.Book.Get10("topmanga"),
                Cate1 = _unitOfWork.Book.Get10("cate1"),
                Cate2 = _unitOfWork.Book.Get10("cate2"),
                Cate3 = _unitOfWork.Book.Get10("cate3")
            };

            return View(viewModel);
        }


    }
}