
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Areas.Admin.Interfaces;
using OnlineShop.Models.Db;


namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class UsersController : Controller
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;

        }

        public async Task<IActionResult> Index()
        {
            return View(await _usersService.GetAllUsersAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _usersService.GetUserByIdAsync(id.Value);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,FullName,Password,IsAdmin,RegisterDate,RecoveryCode")] OnlineShop.Models.Db.User user)
        {
            if (ModelState.IsValid)
            {
                await _usersService.CreateUserAsync(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _usersService.GetUserForEditAsync(id);
            return View(user);
       
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,FullName,Password,IsAdmin,RegisterDate,RecoveryCode")] OnlineShop.Models.Db.User user)
        {
            await _usersService.UpdateUserAsync(id, user);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var user = await _usersService.GetUserForDeleteAsync(id);
            return View(user);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _usersService.DeleteUserAsync(id);
            return RedirectToAction(nameof(Index));
        }


        private bool UserExists(int id)
        {
            return _usersService.UserExists(id);
        }
    }
}
