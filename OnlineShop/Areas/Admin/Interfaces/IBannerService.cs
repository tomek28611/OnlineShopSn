using OnlineShop.Data.Entities;

namespace OnlineShop.Areas.Admin.Interfaces
{
    public interface IBannerService
    {
        Task<List<BannerEntity>> GetAllBannersAsync();
        Task<BannerEntity> GetBannerByIdAsync(int id);
        Task<bool> CreateBannerAsync(BannerEntity banner, IFormFile imageFile);
        Task<BannerEntity?> GetBannerForEditAsync(int id);
        Task<bool> UpdateBannerAsync(BannerEntity banner, IFormFile? imageFile);
        Task<bool> DeleteBannerAsync(int id);
    }
}
