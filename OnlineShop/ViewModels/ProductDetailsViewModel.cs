using OnlineShop.Data;

namespace OnlineShop.ViewModels
{
    public class ProductDetailsViewModel
    {
        public Product? Product { get; set; }
        public List<ProductGalery> Gallery { get; set; } = new();
        public List<Product> NewProducts { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
    }
}
