using OnlineShop.Models.Db;
using Microsoft.EntityFrameworkCore;

public class AdminBannersService
{
    private readonly OnlineShopContext _context;

    public AdminBannersService(OnlineShopContext context)
    {
        _context = context;
    }

    public async Task<List<Banner>> GetAllBannersAsync()
    {
        return await _context.Banners.ToListAsync();
    }
}
