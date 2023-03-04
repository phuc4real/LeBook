using Microsoft.AspNetCore.Mvc;

namespace LeBook.Areas.Admin.Controllers
{
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
