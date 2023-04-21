using AspNetCoreHero.ToastNotification.Abstractions;
using LeBook.DataAccess.Repository.IRepository;
using LeBook.Models;
using LeBook.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LeBookWeb.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize("canView")]
    public class PromotionController : Controller
    {
        private const string PROMOTION = "promotion";
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public INotyfService _notifyService { get; }

        public PromotionController(IUnitOfWork unitOfWork, INotyfService notyfService, IWebHostEnvironment hostEnvironment)
        {
            _notifyService = notyfService;
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;

        }


        // GET: Promotion
        public IActionResult Index()
        {
            return View(_unitOfWork.Promotion.Get(x=>x.IsDeleted == false));
        }

        // GET: Promotion/Upsert
        [Authorize("canAction")]
        public IActionResult Upsert(int? id)
        {
            Promotion promotion;
            ClearSesson();

            if (id == null || id == 0)
            {
                promotion = new();
                return View(promotion);
            }
            else
            {
                promotion = _unitOfWork.Promotion.FirstOrDefault(x => x.Id == id);
                var promotionDetail = _unitOfWork.PromotionDetail.Get(x=>x.PromotionId == id);
                var promotionItems = GetPromotonItems();

                foreach ( var detail in promotionDetail)
                {
                    promotionItems.Add(new ItemModel() { Name = "BookId", Value = detail.BookId });
                }

                SaveSession(promotionItems);
                return View(promotion);
            }
        }

        // POST: Promotion/Upsert
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("canAction")]
        public IActionResult Upsert(Promotion promotion, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRoot = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRoot, @"images\banner");
                    var extension = Path.GetExtension(file.FileName);

                    if (promotion.PromotionBanner != null)
                    {
                        var oldImg = Path.Combine(wwwRoot, promotion.PromotionBanner.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImg))
                        {
                            System.IO.File.Delete(oldImg);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    promotion.PromotionBanner = @"\images\banner\" + fileName + extension;
                }
                if (promotion.Id == 0)
                {
                    _unitOfWork.Promotion.Add(promotion);
                    var promotionItems = GetPromotonItems();
                    foreach (var item in promotionItems)
                    {
                        PromotionDetail promotionDetail = new()
                        {
                            BookId = (int)item.Value,
                            Promotion = promotion,
                        };
                        _unitOfWork.PromotionDetail.Add(promotionDetail);
                    }
                }
                else
                {
                    _unitOfWork.Promotion.Update(promotion);

                    var promotionDetails = _unitOfWork.PromotionDetail.Get(x => x.PromotionId == promotion.Id);
                    _unitOfWork.PromotionDetail.RemoveRange(promotionDetails);

                    var promotionItems = GetPromotonItems();
                    foreach (var item in promotionItems)
                    {
                        PromotionDetail promotionDetail = new()
                        {
                            BookId = (int)item.Value,
                            Promotion = promotion,
                        };
                        _unitOfWork.PromotionDetail.Add(promotionDetail);
                    }
                }

                _unitOfWork.Save();
                _notifyService.Success("Cập nhật thành công!");
                return RedirectToAction(nameof(Index));
            }

            return View(promotion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("canAction")]
        public IActionResult Delete(int id)
        {
            if (_unitOfWork.Promotion == null)
            {
                return base.Problem("Bảng sách trống.");
            }
            var promotion = _unitOfWork.Promotion.FirstOrDefault(x => x.Id == id);
            if (promotion != null && !promotion.IsDeleted)
            {
                _unitOfWork.Promotion.SoftDelete(promotion);
                _notifyService.Success("Xoá khuyến mãi thành công");
            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("canAction")]
        public IActionResult Restore(int? id)
        {
            if (id == null || _unitOfWork.Promotion == null)
            {
                return base.NotFound();
            }

            var promotion = _unitOfWork.Promotion.FirstOrDefault(x => x.Id == id);
            if (promotion != null && promotion.IsDeleted)
            {
                _unitOfWork.Promotion.Restore(promotion);
                _notifyService.Success("Đã phục hồi khuyến mãi");
            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(DeletedIndex));
        }

        public IActionResult DeletedIndex()
        {
            return View(_unitOfWork.Promotion.Get(x => x.IsDeleted == true));
        }

        public IActionResult Detail(int id) 
        {
            Promotion promotion = _unitOfWork.Promotion.FirstOrDefault(x => x.Id == id);
            var promotionDetail = _unitOfWork.PromotionDetail.Get(x => x.PromotionId == id);
            ClearSesson();
            var promotionItems = GetPromotonItems();

            foreach (var detail in promotionDetail)
            {
                promotionItems.Add(new ItemModel() { Name = "BookId", Value = detail.BookId });
            }

            SaveSession(promotionItems);
            return View(promotion);
        }

        public List<ItemModel> GetPromotonItems()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            var session = HttpContext.Session;
            string json = session.GetString(PROMOTION);
            if (json != null)
            {
                return JsonConvert.DeserializeObject<List<ItemModel>>(json);
            }
            return new List<ItemModel>();
        }

        private void ClearSesson()
        {
            var session = HttpContext.Session;
            session.Remove(PROMOTION);
        }

        private void SaveSession(List<ItemModel> ls)
        {
            var session = HttpContext.Session;
            string json = JsonConvert.SerializeObject(ls);
            session.SetString(PROMOTION, json);
        }

        #region API

        [HttpPost]
        public IActionResult AddtoPromotion(int id)
        {

            var promotionItems = GetPromotonItems();
            var item = promotionItems.Find(p => p.Value == id);
            if (item == null)
            {
                promotionItems.Add(new ItemModel() { Name = "BookId", Value = id });
            }

            SaveSession(promotionItems);
            return Ok();
        }

        [HttpPost]
        public IActionResult RemovePromotion(int id)
        {
            var promotionItems = GetPromotonItems();
            var item = promotionItems.Find(p => p.Value == id);
            if (item != null)
            {
                promotionItems.Remove(item);
            }

            SaveSession(promotionItems);
            return Ok();
        }

        [HttpGet]
        public JsonResult PromotionJson()
        {
            var promotionItems = GetPromotonItems();
            return Json(promotionItems);
        }

        #endregion

    }
}
