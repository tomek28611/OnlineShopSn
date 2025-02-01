using Microsoft.AspNetCore.Mvc;
using OnlineShop.ViewModels;

namespace OnlineShop.Controllers
{
    public class CartController : Controller
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        public IActionResult Index()
        {
            var result = _cartService.GetProductsInCart(Request);
            return View("Cart", result);
        }

        [HttpPost]
        public IActionResult UpdateCart([FromBody] CartViewModel request)
        {
            var result = _cartService.UpdateCart(request, Request, Response);
            return result;
        }
         
        public IActionResult SmallCart()
        {
            var result = _cartService.GetProductsInCart(Request);
            return PartialView(result);
        }
    }
}
