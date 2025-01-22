
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Services;


namespace OnlineShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductService _productService;
        private readonly CommentService _commentService;

        public ProductsController(ProductService productService, CommentService commentService)
        {
            _productService = productService;
            _commentService = commentService;
        }

        public IActionResult Index()
        {
            var products = _productService.GetAllProducts();
            return View("Products", products); ;
        }

        public IActionResult SearchProducts(string searchText)
        {
            var products = _productService.SearchProducts(searchText);
            return View("Products", products);
        }

        public IActionResult ProductDetails(int id)
        {
            var productDetails = _productService.GetProductDetails(id);

            if (productDetails.Product == null)
                return NotFound();

            ViewData["gallery"] = productDetails.Gallery;
            ViewData["NewProducts"] = productDetails.NewProducts;
            ViewData["comments"] = productDetails.Comments;

            return View(productDetails.Product);
        }

        [HttpPost]
        public IActionResult SubmitComment(string name, string email, string comment, int productId)
        {
            var result = _commentService.SubmitComment(name, email, comment, productId);
            TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] = result.Message;

            return Redirect($"/Products/ProductDetails/{productId}");
        }
    }
}