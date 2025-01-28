
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models.Db;
using OnlineShop.Areas.Admin.Interfaces;


namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class MenusController : Controller
    {
        private readonly IMenusService _menusService;

        public MenusController(IMenusService menusService)
        {
            _menusService = menusService;
        }

        public async Task<IActionResult> Index()
        {
            var menus = await _menusService.GetAllMenusAsync();
            return View(menus);
        }

        public async Task<IActionResult> Details(int? id)
        {
            var menus = await _menusService.GetMenuDetailsAsync(id);
               
            if (menus == null)
            {
                return NotFound();
            }

            return View(menus);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MenuTitle,Link,Type")] Menus menus)
        {
            if (ModelState.IsValid)
            {
                _menusService.CreateMenuAsync(menus);
                return RedirectToAction(nameof(Index));
            }
            return View(menus);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var menus = await _menusService.GetMenuForEditAsync(id);
            return View(menus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MenuTitle,Link,Type")] Menus menus)
        {
            await _menusService.UpdateMenuAsync(id, menus);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var menus = await _menusService.GetMenuForDeleteAsync(id);
            return View(menus);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _menusService.DeleteMenuAsync(id);
            return RedirectToAction(nameof(Index));
        }
        private bool MenusExists(int id)
        {
            return _menusService.MenuExists(id);
        }
    }
}
