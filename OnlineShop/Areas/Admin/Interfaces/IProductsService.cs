using OnlineShop.Data;

namespace OnlineShop.Areas.Admin.Interfaces
{
    public interface IProductsService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductDetailsAsync(int? id);
        Task CreateProductAsync(Product product, IFormFile? mainImage, IFormFile[]? galleryImages);
        Task<Product> GetProductByIdAsync(int id);
        Task UpdateProductAsync(Product product, IFormFile? mainImage, IFormFile[]? galleryImages);
        Task DeleteProductAsync(int id);
        Task DeleteExistingImageAsync(string imageName, string folder);
    }
}

