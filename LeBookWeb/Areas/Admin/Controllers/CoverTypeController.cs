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
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public INotyfService _notifyService { get; }

        public CoverTypeController(IUnitOfWork unitOfWork, INotyfService notifyService)
        {
            _unitOfWork = unitOfWork;
            _notifyService = notifyService;
        }

        // GET: Admin/CoverType
        public IActionResult Index()
        {
              return View(_unitOfWork.CoverType.Get(x=>x.IsDeleted==false));
        }

        // GET: Admin/CoverType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/CoverType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Add(coverType);
                _unitOfWork.Save();
                _notifyService.Success("Thêm loại bìa thành công");
                return RedirectToAction(nameof(Index));
            }
            return View(coverType);
        }

        // GET: Admin/CoverType/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || _unitOfWork.CoverType == null)
            {
                return NotFound();
            }

            var coverType =  _unitOfWork.CoverType.FirstOrDefault(x => x.Id == id);
            if (coverType == null || coverType.IsDeleted)
            {
                return NotFound();
            }
            return View(coverType);
        }

        // POST: Admin/CoverType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, CoverType coverType)
        {
            if (id != coverType.Id || coverType.IsDeleted)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.CoverType.Update(coverType);
                    _unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoverTypeExists(coverType.Id))
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
            return View(coverType);
        }

        // POST: Admin/CoverType/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            if (_unitOfWork.CoverType == null)
            {
                return Problem("Bảng loại bìa trống.");
            }
            var coverType = _unitOfWork.CoverType.FirstOrDefault(x => x.Id == id);
            if (coverType != null && !coverType.IsDeleted)
            {
                coverType.IsDeleted = true;
                coverType.DeletedAt = DateTime.Now;
            }
            
            _unitOfWork.Save();
            _notifyService.Success("Xoá loại bìa thành công");
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/CoverType/DeletedIndex
        public IActionResult DeletedIndex()
        {
            return View(_unitOfWork.CoverType.Get(x=>x.IsDeleted == true));
        }

        // POST: Admin/CoverType/Restore
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Restore(int? id)
        {
            if (id == null || _unitOfWork.CoverType == null)
            {
                return NotFound();
            }

            var coverType = _unitOfWork.CoverType.FirstOrDefault(x => x.Id == id);
            if (coverType != null && coverType.IsDeleted)
            {
                coverType.IsDeleted = false;
                coverType.LastUpdatedAt = DateTime.Now;
            }

            _unitOfWork.Save();
            _notifyService.Success("Đã phục hồi loại bìa");
            return RedirectToAction(nameof(DeletedIndex));
        }

        private bool CoverTypeExists(int id)
        {
          return _unitOfWork.CoverType.Any(e => e.Id == id);
        }
    }
}
