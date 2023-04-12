using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeBook.Areas.Admin.Controllers
{
    [Authorize("canView")]
    [Area("Admin")]
    public class HomeController : Controller
    {
        //GET: Admin/
        public IActionResult Index()
        {
            return View();
        }
    }
}
