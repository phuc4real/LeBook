using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Helpers;
using LeBook.DataAccess.Repository.IRepository;
using LeBook.Models;
using LeBook.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Security.Claims;
using X.PagedList;

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

        public IActionResult Search(string search, int? page)
        {
            if (search == null) return Redirect("/");
            if (page == null) page = 1;
            var s = search.ToLower();
            var books = _unitOfWork.Book.Get(x => x.IsDeleted == false, includeProperties:"Category1,Category2,Price") ;
            var result = books.Where(b=> b.Title.ToLower().Contains(s) ||
                                         b.Description.ToLower().Contains(s) ||
                                         b.ISBN.Contains(s) ||
                                         b.Author.Contains(s)||
                                         b.Publisher.Contains(s)||
                                         b.Category1.Name.ToLower().Contains(s)||
                                         b.Category2.Name.ToLower().Contains(s))
                            .OrderBy(x => x.Id)
                            .ToList();
            int PageSize = 12;
            int PageNumber = (page ?? 1);
            return View(result.ToPagedList(PageNumber, PageSize));
        }

        public IActionResult Find(string findby, int id, int? page)
        {
            if (page == null) page = 1;
            var books = _unitOfWork.Book.Get(x => x.IsDeleted == false, includeProperties: "Price,Category1,Category2,CoverType,Age");
            switch (findby)
            {
                case "promotion":
                    var promotion = _unitOfWork.PromotionDetail.Get(x => x.Promotion.EndedDate > DateTime.Now && x.Promotion.Id == id, includeProperties: "Promotion");
                    var idList = promotion.Select(x => x.BookId).ToList();
                    books = books.Where(x => idList.Contains(x.Id));
                    break;
                case "cate":
                    var cate = _unitOfWork.Category.FirstOrDefault(x => x.Id == id);
                    books = _unitOfWork.Book.FindByCategory(cate.Id, cate.Level);
                    break;
                case "covertype":
                    var coverType = _unitOfWork.CoverType.FirstOrDefault(x => x.Id == id);
                    books = _unitOfWork.Book.FindByCoverType(coverType.Id);
                    break;
                case "age":
                    var age = _unitOfWork.Age.FirstOrDefault(x => x.Id == id);
                    books = _unitOfWork.Book.FindByAge(age.Id);
                    break;
            }

            int PageSize = 12;
            int PageNumber = (page ?? 1);
            return View(books.ToPagedList(PageNumber, PageSize));
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

        #region API
        [HttpGet]
        public IActionResult GetData(string type)
        {
            switch (type)
            {
                case "cate":
                    var cate = _unitOfWork.Category.Get(x => x.IsDeleted == false);
                    var res = cate.Select(c => new { c.Id, c.Name }).ToList();
                    return Ok(res);
                case "age":
                    var age = _unitOfWork.Age.Get(x => x.IsDeleted == false);
                    res = age.Select(c => new { c.Id, c.Name }).ToList();
                    return Ok(res);
                case "covertype":
                    var covertype = _unitOfWork.CoverType.Get(x => x.IsDeleted == false);
                    res = covertype.Select(c => new { c.Id, c.Name }).ToList();
                    return Ok(res);
                default: return NotFound();
            }
        }


        #endregion
    }
}