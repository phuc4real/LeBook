using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LeBook.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using LeBook.DataAccess;
using LeBook.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;

namespace LeBook.Areas.Admin.Controllers
{
    [Authorize("canView")]
    [Area("Dashboard")]
    public class AgeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public INotyfService _notifyService { get; }
        public AgeController(IUnitOfWork unitOfWork, INotyfService notifyService)
        {
            _unitOfWork = unitOfWork;
            _notifyService = notifyService;
        }

        // GET: Admin/Age
        public IActionResult Index()
        {
              return View( _unitOfWork.Age.Get(x=> x.IsDeleted == false));
        }

        // GET: Admin/Age/Create
        public  IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Age/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Age age)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Age.Add(age);
                _unitOfWork.Save();
                _notifyService.Success("Thêm phân loại độ tuổi thành công");
                return RedirectToAction(nameof(Index));
            }
            return View(age);
        }

        // GET: Admin/Age/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || _unitOfWork.Age == null)
            {
                return NotFound();
            }

            var age =  _unitOfWork.Age.FirstOrDefault(x => x.Id == id);
            if (age == null || age.IsDeleted)
            {
                return NotFound();
            }
            return View(age);
        }

        // POST: Admin/Age/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Age age)
        {
            if (id != age.Id || age.IsDeleted)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.Age.Update(age);
                    _unitOfWork.Save();
                    _notifyService.Success("Phân loại độ tuổi đã được chỉnh sửa");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AgeExists(age.Id))
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
            return View(age);
        }

        // POST: Admin/Age/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            if (_unitOfWork.Age == null)
            {
                return Problem("Bảng độ tuổi trống");
            }
            var age =  _unitOfWork.Age.FirstOrDefault(x => x.Id == id);
            if (age != null && !age.IsDeleted)
            {
                _unitOfWork.Age.SoftDelete(age);
                _notifyService.Success("Xoá phân loại độ tuổi thành công");
            }
            
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Age/DeletedIndex
        public IActionResult DeletedIndex()
        {
            return View( _unitOfWork.Age.Get(x=>x.IsDeleted == true));
        }

        // POST: Admin/Age/Restore/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Restore(int? id)
        {
            if (id == null || _unitOfWork.Age == null)
            {
                return NotFound();
            }

            var age =  _unitOfWork.Age.FirstOrDefault(x => x.Id == id);
            if (age != null && age.IsDeleted)
            {
                _unitOfWork.Age.Restore(age);
                _notifyService.Success("Đã phục hồi độ tuổi");
            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(DeletedIndex));
        }

        private bool AgeExists(int id)
        {
          return _unitOfWork.Age.Any(e => e.Id == id);
        }
    }
}
