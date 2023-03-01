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
            }
            return View(bookView);
        }

        // POST: Admin/Book/Upsert/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(BookViewModel bookView, IFormFile file)
        {

            if (ModelState.IsValid)
            {
                string wwwRoot = _hostEnvironment.WebRootPath;
                if (file != null) {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRoot, @"images\books");
                    var extension = Path.GetExtension(file.FileName);

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    { 
                        file.CopyTo(fileStreams);
                    }
                    bookView.Book.ImgUrl = @"\images\books\" + fileName + extension;
                }

                _unitOfWork.Book.Add(bookView.Book);
                _unitOfWork.Save();
                _notifyService.Success("Thể loại đã được chỉnh sửa");
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
    }
}
