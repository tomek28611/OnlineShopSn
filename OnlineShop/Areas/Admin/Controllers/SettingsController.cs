using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Areas.Admin.Interfaces;
using OnlineShop.Areas.Admin.Services;
using OnlineShop.Data;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class SettingsController : Controller
    {
        private readonly ISettingsService _settingsService;

        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public async Task<IActionResult> Edit()
        {
            try
            {
                var setting = await _settingsService.GetSettingAsync();
                if (setting == null)
                {
                    return NotFound();
                }
                return View(setting);
            }
            catch (ServiceException ex)
            {

                TempData["error"] = "An error occurred while loading settings.";
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,Shipping,Title,Address,Email,Phone,CopyRight,Instagram,FaceBook,GooglePlus,Youtube,Twitter,Logo")]
            Setting setting, IFormFile? newLogo)
        {
            if (id != setting.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var success = await _settingsService.UpdateSettingAsync(setting, newLogo);
                    if (success)
                    {
                        TempData["message"] = "Setting saved successfully.";
                        return RedirectToAction("Edit");
                    }
                }
                catch (ServiceException ex)
                {

                    TempData["error"] = "An error occurred while saving the settings.";
                }
            }

            return View(setting);
        }
    }
}

