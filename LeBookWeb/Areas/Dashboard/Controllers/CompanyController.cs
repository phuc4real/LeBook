using AspNetCoreHero.ToastNotification.Abstractions;
using LeBook.DataAccess.Repository.IRepository;
using LeBook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeBookWeb.Areas.Admin.Controllers
{
    [Authorize("canView")]
    [Area("Dashboard")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public INotyfService _notifyService { get; }

        public CompanyController(IUnitOfWork unitOfWork, INotyfService notifyService)
        {
            _unitOfWork = unitOfWork;
            _notifyService = notifyService;
        }
        // GET: Admin/Company
        public IActionResult Index()
        {
            return View( _unitOfWork.Company.Get(x=>x.IsDeleted == false));
        }
        // GET: Admin/Company/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Company/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Company company)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Company.Add(company);
                _unitOfWork.Save();
                _notifyService.Success("Thêm nhà cung cấp thành công");
                return RedirectToAction(nameof(Index));
            }

            return View(company);
        }

        // GET: Admin/Category/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || _unitOfWork.Company == null)
            {
                return base.NotFound();
            }

            var company = _unitOfWork.Company.FirstOrDefault(x => x.Id == id);
            if (company == null || company.IsDeleted)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: Admin/Company/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Company company)
        {
            if (id != company.Id || company.IsDeleted)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.Company.Update(company);
                    _unitOfWork.Save();
                    _notifyService.Success("Nhà cung cấp đã được chỉnh sửa");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.Id))
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
            return View(company);
        }

        // POST: Admin/Company/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            if (_unitOfWork.Company == null)
            {
                return base.Problem("Bảng nhà cung cấp trống.");
            }
            var company = _unitOfWork.Company.FirstOrDefault(x => x.Id == id);
            if (company != null && !company.IsDeleted)
            {
                _unitOfWork.Company.SoftDelete(company);
                _notifyService.Success("Xoá nhà cung cấp thành công");
            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Company/DeletedIndex
        public IActionResult DeletedIndex()
        {
            return View( _unitOfWork.Company.Get(x=> x.IsDeleted == true));
        }

        // POST: Admin/Company/Restore/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Restore(int? id)
        {
            if (id == null || _unitOfWork.Company == null)
            {
                return base.NotFound();
            }

            var company = _unitOfWork.Company.FirstOrDefault(x => x.Id == id);
            if (company != null && company.IsDeleted)
            {
                _unitOfWork.Company.Restore(company);
                _notifyService.Success("Đã phục hồi nhà cung cấp");
            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(DeletedIndex));
        }

        private bool CompanyExists(int id)
        {
            return _unitOfWork.Company.Any(e => e.Id == id);
        }
    }
}