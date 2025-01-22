using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models;


namespace OnlineShop.Controllers
{
    public class HomeController : Controller
    {

        private readonly AdminBannersService _bannerService;

        public HomeController(AdminBannersService bannerService)
        {

            _bannerService = bannerService;
        }

        public async Task<IActionResult> Index()
        {
            var banners = await _bannerService.GetAllBannersAsync();
            ViewData["banners"] = banners;
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
