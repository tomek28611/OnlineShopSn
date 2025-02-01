using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.ViewModels;

namespace OnlineShop.Services
{
    public class ProductService
    {
        private readonly OnlineShopContext _context;

        public ProductService(OnlineShopContext context)
        {
            _context = context;
        }

        public List<Product> GetAllProducts()
        {
            return _context.Products.OrderByDescending(x => x.Id).ToList();
        }

        public List<Product> SearchProducts(string searchText)
        {
            return _context.Products
                .Where(x =>
                    EF.Functions.Like(x.Title, $"%{searchText}%") ||
                    EF.Functions.Like(x.Tags, $"%{searchText}%"))
                .OrderBy(x => x.Title)
                .ToList();
        }

        public ProductDetailsViewModel GetProductDetails(int id)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == id);
            var gallery = _context.ProductGaleries.Where(x => x.ProductId == id).ToList();
            var newProducts = _context.Products.Where(x => x.Id != id).Take(6).OrderByDescending(x => x.Id).ToList();
            var comments = _context.Comments.Where(x => x.ProductId == id).OrderByDescending(x => x.CreateDate).ToList();

            return new ProductDetailsViewModel
            {
                Product = product,
                Gallery = gallery,
                NewProducts = newProducts,
                Comments = comments
            };
        }
    }
}
