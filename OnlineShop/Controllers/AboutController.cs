using Microsoft.AspNetCore.Mvc;

namespace OnlineShop.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
