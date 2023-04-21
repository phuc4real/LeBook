using AspNetCoreHero.ToastNotification.Abstractions;
using LeBook.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LeBookWeb.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize("canView")]
    public class PromotionController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public INotyfService _notifyService { get; }

        public PromotionController(IUnitOfWork unitOfWork, INotyfService notyfService)
        {
            _notifyService = notyfService;
            _unitOfWork = unitOfWork;
        }


        // GET: Promotion
        public ActionResult Index()
        {
            return View(_unitOfWork.Promotion.Get());
        }

        // GET: Promotion/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Promotion/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Promotion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Promotion/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Promotion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Promotion/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Promotion/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
