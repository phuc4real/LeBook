using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LeBook.DataAccess;
using LeBook.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using LeBook.DataAccess.Repository.IRepository;
using LeBook.Models.ViewModel;
using LeBook.DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace LeBook.Areas.Admin.Controllers
{
    [Authorize("canView")]
    [Area("Dashboard")]
    public class BookController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public INotyfService _notifyService { get; }

        public BookController(IUnitOfWork unitOfWork, INotyfService notifyService, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _notifyService = notifyService;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Admin/Book
        public IActionResult Index()
        {
            return View( _unitOfWork.Book.Get(x=>x.IsDeleted == false,includeProperties: "Price,Category1,Category2,CoverType,Age"));
        }
        
        // GET: Admin/Book/Upsert/5
        public IActionResult Detail(int id)
        {
            BookViewModel bookView = new()
            {
                Book = _unitOfWork.Book.GetFirst(id),
                Category1List = _unitOfWork.Category.Get(x => x.IsDeleted == false && x.Level == 1).Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }),
                Category2List = _unitOfWork.Category.Get(x => x.IsDeleted == false && x.Level == 2).Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }),
                CoverTypeList = _unitOfWork.CoverType.Get(x => x.IsDeleted == false).Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }),
                AgeList = _unitOfWork.Age.Get(x => x.IsDeleted == false).Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }),
            };

            bookView.itemPrice = bookView.Book.Price.OrderByDescending(p => p.Id).First().ItemPrice;

            return View(bookView);
        }
        [Authorize("canAction")]
        // GET: Admin/Book/Upsert/5
        public  IActionResult Upsert(int? id)
        {
            BookViewModel bookView = new()
            {
                Book = new(),

                Category1List = _unitOfWork.Category.Get(x => x.IsDeleted == false && x.Level == 1).Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }),
                Category2List = _unitOfWork.Category.Get(x => x.IsDeleted == false && x.Level == 2).Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }),
                CoverTypeList = _unitOfWork.CoverType.Get(x => x.IsDeleted == false).Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }),
                AgeList = _unitOfWork.Age.Get(x => x.IsDeleted == false).Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }),
            };
             
            if (id == null || id == 0 )
            {
                //Create Book
                return View(bookView);
            }
            else
            {
                //Update Book
                bookView.Book = _unitOfWork.Book.GetFirst(id);
                if (bookView.Book.Price.Count > 0 )
                {
                    bookView.itemPrice = bookView.Book.Price.OrderByDescending(p => p.Id).First().ItemPrice;
                }
                else
                {
                    bookView.itemPrice = 0;
                }

                return View(bookView);
            }
        }

        // POST: Admin/Book/Upsert/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("canAction")]
        public IActionResult Upsert(BookViewModel bookView, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                string wwwRoot = _hostEnvironment.WebRootPath;
                if (file != null) {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRoot, @"images\books");
                    var extension = Path.GetExtension(file.FileName);

                    if (bookView.Book.ImgUrl != null)
                    {
                        var oldImg = Path.Combine(wwwRoot,bookView.Book.ImgUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImg))
                        {
                            System.IO.File.Delete(oldImg);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    { 
                        file.CopyTo(fileStreams);
                    }
                    bookView.Book.ImgUrl = @"\images\books\" + fileName + extension;
                }



                if (bookView.Book.Id == 0)
                { 
                    _unitOfWork.Book.Add(bookView.Book);
                    Price price = new()
                    {
                        ItemPrice = bookView.itemPrice,
                        Date = DateTime.Now,
                        Book = bookView.Book
                    };
                    _unitOfWork.Price.Add(price);
                }
                else
                { 
                    _unitOfWork.Book.Update(bookView.Book);
                    Price price = new()
                    {
                        ItemPrice = bookView.itemPrice,
                        Date = DateTime.Now,
                        BookId = bookView.Book.Id
                    };
                    _unitOfWork.Price.Add(price);
                }

                _unitOfWork.Save();
                _notifyService.Success("Cập nhật thành công!");
                return RedirectToAction(nameof(Index));
            }
            return View(bookView);
        }


        // GET: Admin/Book/List/1?bookby=cate
        public IActionResult List(string bookby, int? Id)
        {
            IEnumerable<Book> booklist = _unitOfWork.Book.Get(x => x.IsDeleted == false, includeProperties: "Price,Category1,Category2,CoverType,Age");

            if (Id == null)
            {
                switch (bookby)
                {
                    case "sell":
                        booklist = booklist.OrderByDescending(x => x.Sold); ViewBag.BookBy = "theo số lượng bán"; break;
                    case "stock":
                        booklist = booklist.OrderByDescending(x => x.InStock); ViewBag.BookBy = "theo số lượng tồn"; break;
                }
            }
            else
            {
                switch (bookby)
                {
                    case "cate":
                        var cate = _unitOfWork.Category.FirstOrDefault(x => x.Id == Id);
                        ViewBag.BookBy = "theo thể loại: " + cate.Name;
                        booklist = _unitOfWork.Book.FindByCategory(cate.Id, cate.Level);
                        break;
                    case "covertype":
                        var coverType = _unitOfWork.CoverType.FirstOrDefault(x => x.Id == Id);
                        ViewBag.BookBy = "theo loại bìa: " + coverType.Name;
                        booklist = _unitOfWork.Book.FindByCoverType(coverType.Id);
                        break;
                    case "age":
                        var age = _unitOfWork.Age.FirstOrDefault(x => x.Id == Id);
                        ViewBag.BookBy = "theo độ tuổi: " + age.Name;
                        booklist = _unitOfWork.Book.FindByAge(age.Id);
                        break;
                    default:
                        return NotFound();
                }
            }

            return View("Index", booklist);
        }

        // POST: Admin/Book/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("canAction")]
        public IActionResult Delete(int id)
        {
            if (_unitOfWork.Book == null)
            {
                return base.Problem("Bảng sách trống.");
            }
            var book = _unitOfWork.Book.FirstOrDefault(x => x.Id == id);
            if (book != null && !book.IsDeleted)
            {
                _unitOfWork.Book.SoftDelete(book);
                _notifyService.Success("Xoá sách thành công");
            }
            
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Book/DeletedIndex
        public IActionResult DeletedIndex()
        {
            return View(_unitOfWork.Book.Get(x=>x.IsDeleted==true));
        }

        // POST: Admin/Book/Restore/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("canAction")]
        public IActionResult Restore(int? id)
        {
            if (id == null || _unitOfWork.Book == null)
            {
                return base.NotFound();
            }

            var book = _unitOfWork.Book.FirstOrDefault(x => x.Id == id);
            if (book != null && book.IsDeleted)
            {
                _unitOfWork.Book.Restore(book);
                _notifyService.Success("Đã phục hồi sách");
            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(DeletedIndex));
        }

        private bool BookExists(int id)
        {
          return _unitOfWork.Book.Any(e => e.Id == id);
        }

        #region API
        [HttpGet]
        public IActionResult GetBook()
        {
            var bookList = _unitOfWork.Book.Get();
            return Json(bookList);

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult BookList()
        {
            var books = _unitOfWork.Book.Get();
            var list = books.Select(x => new { x.Id, x.Title }).ToList();
            return Json(list);

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult BookIdbyName()
        {
            var books = _unitOfWork.Book.Get();
            var list = books.Select(x => new { x.Id, x.Title }).ToList();
            return Json(list);

        }

        public IActionResult GetBookWithPrice(int id)
        {
            var bookList = _unitOfWork.Book.GetFirst(id);
            return Json(bookList);

        }
        #endregion
    }
}
