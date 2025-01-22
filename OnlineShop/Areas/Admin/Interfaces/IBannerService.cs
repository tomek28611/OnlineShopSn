using OnlineShop.Models.Db;

namespace OnlineShop.Areas.Admin.Interfaces
{
    public interface IBannerService
    {
        Task<List<Banner>> GetAllBannersAsync();
        Task<Banner> GetBannerByIdAsync(int id);
        Task<bool> CreateBannerAsync(Banner banner, IFormFile imageFile);
        Task<Banner?> GetBannerForEditAsync(int id);
        Task<bool> UpdateBannerAsync(Banner banner, IFormFile? imageFile);
        Task<bool> DeleteBannerAsync(int id);
    }
}
