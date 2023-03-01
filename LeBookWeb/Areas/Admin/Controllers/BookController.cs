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

namespace LeBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public INotyfService _notifyService { get; }

        public BookController(IUnitOfWork unitOfWork, INotyfService notifyService)
        {
            _unitOfWork = unitOfWork;
            _notifyService = notifyService;
        }

        // GET: Admin/Book
        public IActionResult Index()
        {
            return View( _unitOfWork.Book.Get());
        }

        // GET: Admin/Book/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Book.Add(book);
                _unitOfWork.Save();
                _notifyService.Success("Thêm thể loại thành công");
                return RedirectToAction(nameof(Index));
            }

            return View(book);
        }

        // GET: Admin/Book/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || _unitOfWork.Book == null)
            {
                return base.NotFound();
            }

            var book = _unitOfWork.Book.GetFirtOrDefault(x => x.Id == id);
            if (book == null || book.IsDeleted)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Admin/Book/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Book book)
        {
            if (id != book.Id || book.IsDeleted)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.Book.Update(book);
                    _unitOfWork.Save();
                    _notifyService.Success("Thể loại đã được chỉnh sửa");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return View(book);
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
