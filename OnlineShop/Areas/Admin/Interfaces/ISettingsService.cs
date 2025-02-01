using OnlineShop.Data;

namespace OnlineShop.Areas.Admin.Interfaces
{
    public interface ISettingsService
    {
        Task<Setting> GetSettingAsync();
        Task<bool> UpdateSettingAsync(Setting setting, IFormFile? newLogo);
        bool SettingExists(int id);
    }
}
