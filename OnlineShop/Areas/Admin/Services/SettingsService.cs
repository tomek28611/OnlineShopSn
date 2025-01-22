using Microsoft.EntityFrameworkCore;
using OnlineShop.Areas.Admin.Interfaces;
using OnlineShop.Exceptions;
using OnlineShop.Models.Db;
using System;

namespace OnlineShop.Areas.Admin.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly OnlineShopContext _context;

        public SettingsService(OnlineShopContext context)
        {
            _context = context;
        }

        public async Task<Setting> GetSettingAsync()
        {
            try
            {
                return await _context.Settings.FirstAsync();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error retrieving settings.", ex);
            }
        }

        public async Task<bool> UpdateSettingAsync(Setting setting, IFormFile? newLogo)
        {
            try
            {
                if (newLogo != null)
                {
                    string directory = Directory.GetCurrentDirectory();
                    string oldPath = Path.Combine(directory, "wwwroot", "images", setting.Logo);

                    if (File.Exists(oldPath))
                    {
                        File.Delete(oldPath);
                    }

                    setting.Logo = Guid.NewGuid() + Path.GetExtension(newLogo.FileName);
                    string newPath = Path.Combine(directory, "wwwroot", "images", setting.Logo);

                    using (var stream = new FileStream(newPath, FileMode.Create))
                    {
                        newLogo.CopyTo(stream);
                    }
                }

                _context.Update(setting);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new SettingUpdateException("Error during setting update", ex);
            }
        }

        public bool SettingExists(int id)
        {
            try
            {
                return _context.Settings.Any(e => e.Id == id);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error checking if the setting exists.", ex);
            }
        }
    }

    public class ServiceException : Exception
    {
        public ServiceException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}

