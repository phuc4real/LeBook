using AspNetCoreHero.ToastNotification.Abstractions;
using LeBook.DataAccess.Repository.IRepository;
using LeBook.Models;
using LeBook.Models.ViewModel;
using LeBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;
using System.Security.Claims;

namespace LeBookWeb.Areas.Admin.Controllers
{
    [Authorize("canView")]
    [Area("Dashboard")]
    public class PurchaseOrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public INotyfService _notifyService { get; }

        private const string PURCHASE = "purchase";

        public PurchaseOrderController(IUnitOfWork unitOfWork, INotyfService notifyService)
        {
            _unitOfWork = unitOfWork;
            _notifyService = notifyService;
        }

        public IActionResult Index()
        {
            return View(_unitOfWork.PurchaseOrder.Get(includeProperties: "User,Company"));
        }

        public IActionResult Detail(int id)
        {
            var o = _unitOfWork.PurchaseOrder.FirstOrDefault(x => x.Id == id, includeProperties: "User,Company");
            PurchaseViewModel viewModel = new()
            {
                PurchaseOrder = o,
                PurchaseDetails = _unitOfWork.PurchaseDetail.Get(x => x.PurchaseId == id, includeProperties: "Book"),
            };

            return View(viewModel);
        }

        [HttpGet]
        [Authorize("canAction")]
        public IActionResult Create()
        {
            PurchaseViewModel viewModel = new()
            {
                PurchaseOrder = new(),
                Company = _unitOfWork.Company.Get(x=>x.IsDeleted == false).Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }),
            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize("canAction")]
        public IActionResult Create(PurchaseViewModel viewModel)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            PurchaseOrder purchaseOrder = new PurchaseOrder()
            {
                UserId = claim.Value,
                CompanyId = viewModel.PurchaseOrder.CompanyId,
                PurchaseTotal = viewModel.PurchaseOrder.PurchaseTotal
            };
            _unitOfWork.PurchaseOrder.Add(purchaseOrder);

            var ListPurchase = GetPurchaseItems();
            foreach (var item in ListPurchase)
            {
                PurchaseDetail purchaseDetail = new PurchaseDetail()
                {
                    Price = item.Price,
                    BookId = item.BookId,
                    Quantity = item.Quantity,
                    Total = item.Price * item.Quantity,
                    Purchase = purchaseOrder
                };

                _unitOfWork.Book.AddBookStock(item.BookId, item.Quantity);
                _unitOfWork.PurchaseDetail.Add(purchaseDetail);
            }

            _unitOfWork.Save();
            ClearSesson();
            return RedirectToAction("Index");
        }

        public List<PurchaseItem> GetPurchaseItems()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            var session = HttpContext.Session;
            string json = session.GetString(PURCHASE);
            if (json != null)
            {
                return JsonConvert.DeserializeObject<List<PurchaseItem>>(json);
            }
            return new List<PurchaseItem>();
        }

        private void ClearSesson()
        {
            var session = HttpContext.Session;
            session.Remove(PURCHASE);
        }

        private void SaveSession(List<PurchaseItem> ls)
        {
            var session = HttpContext.Session;
            string json = JsonConvert.SerializeObject(ls);
            session.SetString(PURCHASE, json);
        }

        #region API

        [HttpPost]
        public IActionResult AddToPurchase(int id, int quantity, double price)
        {

            var purchaseItems = GetPurchaseItems();
            var item = purchaseItems.Find(p => p.BookId == id);
            if (item != null)
            {
                item.Quantity++;
                item.Price = price;
            }
            else
            {
                purchaseItems.Add(new PurchaseItem() { Quantity = quantity, BookId = id, Price = price });
            }

            SaveSession(purchaseItems);
            return Ok();
        }

        [HttpPost]
        public IActionResult RemovePurchase(int id)
        {
            var purchaseItems = GetPurchaseItems();
            var item = purchaseItems.Find(p => p.BookId == id);
            if (item != null)
            {
                purchaseItems.Remove(item);
            }

            SaveSession(purchaseItems);
            return Ok();
        }

        [HttpGet]
        public JsonResult PurchaseJson()
        {
            var purchaseItems = GetPurchaseItems();
            return Json(purchaseItems);
        }

        #endregion
    }
}
