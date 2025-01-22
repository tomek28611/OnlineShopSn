
using Microsoft.EntityFrameworkCore;
using OnlineShop.Areas.Admin.Interfaces;
using OnlineShop.Models.Db;


namespace OnlineShop.Areas.Admin.Services
{
    public class ProductsService : IProductsService
    {
        private readonly OnlineShopContext _context;

        public ProductsService(OnlineShopContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetProductDetailsAsync(int? id)
        {
            if (id == null)
                return null;

            return await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task CreateProductAsync(Product product, IFormFile? mainImage, IFormFile[]? galleryImages)
        {
            if (mainImage != null)
            {
                product.ImageName = await SaveImageAsync(mainImage, "banners");
            }

            await SaveProductAsync(product);

            if (galleryImages != null && galleryImages.Any())
            {
                await SaveGalleryImagesAsync(product.Id, galleryImages);
            }
        }

        public async Task UpdateProductAsync(Product product, IFormFile? mainImage, IFormFile[]? galleryImages)
        {
           
            if (mainImage != null)
            {
                await DeleteExistingImageAsync(product.ImageName, "banners");
                product.ImageName = await SaveImageAsync(mainImage, "banners");
            }

            if (galleryImages != null && galleryImages.Any())
            {
                await SaveGalleryImagesAsync(product.Id, galleryImages);
            }

            _context.Update(product);
            await _context.SaveChangesAsync();
        }

        private async Task<string> SaveImageAsync(IFormFile image, string folder)
        {
            string imageName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", folder, imageName);

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return imageName;
        }

        private async Task SaveProductAsync(Product product)
        {
            _context.Add(product);
            await _context.SaveChangesAsync();
        }

        private async Task SaveGalleryImagesAsync(int productId, IFormFile[] galleryImages)
        {
            foreach (var image in galleryImages)
            {
                var galleryImage = new ProductGalery
                {
                    ProductId = productId,
                    ImageName = await SaveImageAsync(image, "banners")
                };

                _context.ProductGaleries.Add(galleryImage);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteExistingImageAsync(string imageName, string folder)
        {
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", folder, imageName);
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
              
                string mainImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "banners", product.ImageName);
                if (File.Exists(mainImagePath))
                {
                    File.Delete(mainImagePath);
                }

                var galleries = _context.ProductGaleries.Where(x => x.ProductId == id).ToList();
                if (galleries != null && galleries.Any())
                {
                    foreach (var item in galleries)
                    {
                        string galleryImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "banners", item.ImageName);
                        if (File.Exists(galleryImagePath))
                        {
                            File.Delete(galleryImagePath);
                        }
                    }

                    _context.ProductGaleries.RemoveRange(galleries);
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
