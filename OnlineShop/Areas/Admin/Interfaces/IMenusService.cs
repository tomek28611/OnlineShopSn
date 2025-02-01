using OnlineShop.Data;

namespace OnlineShop.Areas.Admin.Interfaces
{
    public interface IMenusService
    {
        Task<IEnumerable<Menus>> GetAllMenusAsync();
        Task<Menus?> GetMenuDetailsAsync(int? id);
        Task<bool> CreateMenuAsync(Menus menus);
        Task<Menus?> GetMenuForEditAsync(int? id);
        Task<bool> UpdateMenuAsync(int id, Menus menus);
        Task<Menus?> GetMenuForDeleteAsync(int? id);
        Task<bool> DeleteMenuAsync(int id);
        bool MenuExists(int id);
    }
}
