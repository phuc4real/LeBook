using AspNetCoreHero.ToastNotification.Abstractions;
using LeBook.DataAccess.Repository.IRepository;
using LeBook.Models.ViewModel;
using LeBook.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.Extensions.Hosting;

namespace LeBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class AddressController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public INotyfService _notifyService { get; }

        public AddressController(IUnitOfWork unitOfWork, INotyfService notyfService)
        {
            _notifyService = notyfService;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var res = _unitOfWork.UserAddress.Get(x => x.UserId == claim.Value && x.IsDeleted == false);
            return View(res);
        }

        public ActionResult Upsert(int? id)
        {
            UserAddress address;

            if (id == null || id == 0)
            {
                address = new();
            }
            else
            {
                address = _unitOfWork.UserAddress.FirstOrDefault(x => x.Id == id);
            }
            return View(address);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upsert(UserAddress address)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (address != null)
            {
                address.UserId = claim.Value;
                if (address.Id == 0)
                {
                    _unitOfWork.UserAddress.Add(address);
                }
                else
                {
                    _unitOfWork.UserAddress.Update(address);
                }

                _unitOfWork.Save();
                _notifyService.Success("Cập nhật thành công!");
                return RedirectToAction(nameof(Index));
            }
            return View(address);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            if (_unitOfWork.UserAddress == null)
            {
                return base.Problem("Không có địa chỉ giao hàng.");
            }
            var address = _unitOfWork.UserAddress.FirstOrDefault(x => x.Id == id);
            if (address != null && !address.IsDeleted)
            {
                _unitOfWork.UserAddress.SoftDelete(address);
                _notifyService.Success("Xoá địa chỉ thành công");
            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}
