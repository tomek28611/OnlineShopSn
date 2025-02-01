using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using OnlineShop.Data;
using OnlineShop.Data.Entities;

public class AdminBannersRepository
{
    private readonly IDbContextFactory<OnlineShopContext> _contextFactory;

    public AdminBannersRepository(IDbContextFactory<OnlineShopContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<List<BannerEntity>> GetAllBannersAsync()
    {

        using var _context = await _contextFactory.CreateDbContextAsync();
    }
}
