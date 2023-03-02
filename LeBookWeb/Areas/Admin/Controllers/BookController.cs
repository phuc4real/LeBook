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

namespace LeBook.Areas.Admin.Controllers
{
    [Area("Admin")]
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
            return View( _unitOfWork.Book.Get());
        }

        // GET: Admin/Book/Upsert/5
        public IActionResult Upsert(int? id)
        {
            BookViewModel bookView = new()
            {
                Book = new(),
                CategoryList = _unitOfWork.Category.Get().Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }),
                CoverTypeList = _unitOfWork.CoverType.Get().Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }),
                AgeList = _unitOfWork.Age.Get().Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }),
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
                    bookView.itemPrice = bookView.Book.Price.OrderByDescending(p => p.Id).FirstOrDefault().ItemPrice;
                }
                else
                {
                    bookView.itemPrice = 0;
                }

                return View(bookView);
            }
            return View(bookView);
        }

        // POST: Admin/Book/Upsert/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // POST: Admin/Book/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            if (_unitOfWork.Book == null)
            {
                return base.Problem("Bảng thể loại trống.");
            }
            var book = _unitOfWork.Book.GetFirtOrDefault(x => x.Id == id);
            if (book != null && !book.IsDeleted)
            {
                _unitOfWork.Book.SoftDelete(book);
                _notifyService.Success("Xoá thể loại thành công");
            }
            
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Book/DeletedIndex
        public IActionResult DeletedIndex()
        {
            return View( _unitOfWork.Book.GetDeleted());
        }

        // POST: Admin/Book/Restore/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Restore(int? id)
        {
            if (id == null || _unitOfWork.Book == null)
            {
                return base.NotFound();
            }

            var book = _unitOfWork.Book.GetFirtOrDefault(x => x.Id == id);
            if (book != null && book.IsDeleted)
            {
                _unitOfWork.Book.Restore(book);
                _notifyService.Success("Đã phục hồi thể loại");
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

        public IActionResult GetBookWithPrice(int id)
        {
            var bookList = _unitOfWork.Book.GetFirst(id);
            return Json(bookList);

        }
        #endregion
    }
}
