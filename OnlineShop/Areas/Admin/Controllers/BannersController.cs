
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Areas.Admin.Interfaces;
using OnlineShop.Data.Entities;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class BannersController : Controller
    {
        private readonly IBannerService _bannerService;

        public BannersController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        public async Task<IActionResult> Index()
        {
            var banners = await _bannerService.GetAllBannersAsync();
            return View(banners);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _bannerService.GetBannerByIdAsync(id.Value);
            if (banner == null)
            {
                return NotFound();
            }

            return View(banner);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,SubTitle,ImageName,Priority,Link,Position")] BannerEntity banner, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                var result = await _bannerService.CreateBannerAsync(banner, imageFile);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(banner);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _bannerService.GetBannerForEditAsync(id.Value);
            if (banner == null)
            {
                return NotFound();
            }

            return View(banner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,SubTitle,ImageName,Priority,Link,Position")] BannerEntity banner, IFormFile? imageFile)
        {
            if (id != banner.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _bannerService.UpdateBannerAsync(banner, imageFile);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                return NotFound();
            }

            return View(banner);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _bannerService.GetBannerByIdAsync(id.Value);
            if (banner == null)
            {
                return NotFound();
            }

            return View(banner);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _bannerService.DeleteBannerAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }
    }
}

