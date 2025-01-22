
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Areas.Admin.Interfaces;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Models.Db;
using OnlineShop.Areas.Admin.Services;


namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class ProductsController : Controller
    {
        private readonly OnlineShopContext _context;
        private readonly IProductsService _productsService;

        public ProductsController(OnlineShopContext context, IProductsService productsService)
        {
            _context = context;
            _productsService = productsService;
        }


        public async Task<IActionResult> Index()
        {
            var menus = await _productsService.GetAllProductsAsync();
            return View(menus);
        }

        public IActionResult DeleteGallery(int id)
        {
            var gallery = _context.ProductGaleries.FirstOrDefault(x => x.Id == id);
            if (gallery == null)
            {
                return NotFound();
            }
            string d = Directory.GetCurrentDirectory();
            string fn = d + "\\wwwroot\\images\\banners\\" + gallery.ImageName;

            if (System.IO.File.Exists(fn))
            {
                System.IO.File.Delete(fn);
            }
            _context.Remove(gallery);
            _context.SaveChanges();

            return Redirect("edit/" + gallery.ProductId);
        }


        public async Task<IActionResult> Details(int? id)
        {
            var product = await _productsService.GetProductDetailsAsync(id);
            return View(product);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,FullDesc,Price,Discount,ImageName,Qty,Tags,VideoUrl")] Product product, IFormFile? MainImage, IFormFile[]? GalleryImages)
        {
            if (ModelState.IsValid)
            {
                await _productsService.CreateProductAsync(product, MainImage, GalleryImages);
                return RedirectToAction(nameof(Index));
           
            }
            return View(product);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productsService.GetProductByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            ViewData["gallery"] = await _context.ProductGaleries.Where(x => x.ProductId == product.Id).ToListAsync();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,FullDesc,Price,Discount,ImageName,Qty,Tags,VideoUrl")] Product product, IFormFile? MainImage, IFormFile[]? GalleryImages)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _productsService.UpdateProductAsync(product, MainImage, GalleryImages);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Products.Any(p => p.Id == product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productsService.GetProductByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productsService.DeleteProductAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
