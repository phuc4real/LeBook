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
using Microsoft.AspNetCore.Authorization;

namespace LeBook.Areas.Admin.Controllers
{
    [Authorize("canView")]
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public INotyfService _notifyService { get; }

        public CategoryController(IUnitOfWork unitOfWork, INotyfService notifyService)
        {
            _unitOfWork = unitOfWork;
            _notifyService = notifyService;
        }

        // GET: Admin/Category
        public IActionResult Index()
        {
            return View(_unitOfWork.Category.Get(x => x.IsDeleted == false));
        }

        // GET: Admin/Category/Create
        [Authorize("canAction")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("canAction")]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(category);
                _unitOfWork.Save();
                _notifyService.Success("Thêm thể loại thành công");
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // GET: Admin/Category/Edit/5
        [Authorize("canAction")]
        public  IActionResult Edit(int? id)
        {
            if (id == null || _unitOfWork.Category == null)
            {
                return base.NotFound();
            }

            var category = _unitOfWork.Category.FirstOrDefault(x => x.Id == id);
            if (category == null || category.IsDeleted)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Admin/Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("canAction")]
        public IActionResult Edit(int id, Category category)
        {
            if (id != category.Id || category.IsDeleted)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.Category.Update(category);
                    _unitOfWork.Save();
                    _notifyService.Success("Thể loại đã được chỉnh sửa");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("canAction")]
        public IActionResult Delete(int id)
        {
            if (_unitOfWork.Category == null)
            {
                return base.Problem("Bảng thể loại trống.");
            }
            var category = _unitOfWork.Category.FirstOrDefault(x => x.Id == id);
            if (category != null && !category.IsDeleted)
            {
                _unitOfWork.Category.SoftDelete(category);
                _notifyService.Success("Xoá thể loại thành công");
            }
            
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Category/DeletedIndex
        public IActionResult DeletedIndex()
        {
            return View( _unitOfWork.Category.Get(x => x.IsDeleted == true));
        }

        // POST: Admin/Category/Restore/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("canAction")]
        public IActionResult Restore(int? id)
        {
            if (id == null || _unitOfWork.Category == null)
            {
                return base.NotFound();
            }

            var category = _unitOfWork.Category.FirstOrDefault(x => x.Id == id);
            if (category != null && category.IsDeleted)
            {
                _unitOfWork.Category.Restore(category);
                _notifyService.Success("Đã phục hồi thể loại");
            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(DeletedIndex));
        }

        private bool CategoryExists(int id)
        {
          return _unitOfWork.Category.Any(e => e.Id == id);
        }
    }
}
