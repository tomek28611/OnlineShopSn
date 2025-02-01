
using Microsoft.EntityFrameworkCore;
using OnlineShop.Areas.Admin.Interfaces;
using OnlineShop.Data;
using OnlineShop.Data.Entities;

namespace OnlineShop.Areas.Admin.Interfaces
{

    public class BannerService : IBannerService
    {
        private readonly OnlineShopContext _context;

        public BannerService(OnlineShopContext context)
        {
            _context = context;
        }

        public async Task<List<BannerEntity>> GetAllBannersAsync()
        {
            return await _context.Banners.ToListAsync();
        }

        public async Task<BannerEntity> GetBannerByIdAsync(int id)
        {
            return await _context.Banners.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<bool> CreateBannerAsync(BannerEntity banner, IFormFile imageFile)
        {
            if (imageFile != null)
            {
                banner.ImageName = SaveImage(imageFile);
            }

            _context.Banners.Add(banner);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<BannerEntity?> GetBannerForEditAsync(int id)
        {
            return await _context.Banners.FindAsync(id);
        }


        public async Task<bool> UpdateBannerAsync(BannerEntity banner, IFormFile? imageFile)
        {
            var existingBanner = await _context.Banners.FindAsync(banner.Id);
            if (existingBanner == null)
            {
                return false;
            }

            try
            {               
                if (imageFile != null)
                {
                 
                    if (!string.IsNullOrEmpty(existingBanner.ImageName))
                    {
                        string existingImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/banners", existingBanner.ImageName);
                        if (System.IO.File.Exists(existingImagePath))
                        {
                            System.IO.File.Delete(existingImagePath);
                        }
                    }

                    string newImageName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                    string newImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/banners", newImageName);

                    using (var stream = new FileStream(newImagePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    existingBanner.ImageName = newImageName;
                }

                existingBanner.Title = banner.Title;
                existingBanner.SubTitle = banner.SubTitle;
                existingBanner.Priority = banner.Priority;
                existingBanner.Link = banner.Link;
                existingBanner.Position = banner.Position;

                _context.Banners.Update(existingBanner);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await BannerExistsAsync(banner.Id))
                {
                    return false;
                }
                throw;
            }
        }

        private async Task<bool> BannerExistsAsync(int id)
        {
            return await _context.Banners.AnyAsync(e => e.Id == id);
        }

        public async Task<bool> DeleteBannerAsync(int id)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner == null)
            {
                return false;
            }

            DeleteImage(banner.ImageName);
            _context.Banners.Remove(banner);
            await _context.SaveChangesAsync();
            return true;
        }

        private string SaveImage(IFormFile imageFile)
        {
            var imageName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/banners", imageName);

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                imageFile.CopyTo(stream);
            }

            return imageName;
        }

        private void DeleteImage(string imageName)
        {
            if (string.IsNullOrEmpty(imageName)) return;

            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/banners", imageName);
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }
    }
}

