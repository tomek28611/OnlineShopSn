using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlineShop.Models.Db;
using OnlineShop.Models.ViewModels;

public class CartService
{
    private readonly OnlineShopContext _context;

    public CartService(OnlineShopContext context)
    {
        _context = context;
    }

    public List<CartViewModel> GetCartItems(HttpRequest request)
    {
        List<CartViewModel> cartList = new List<CartViewModel>();

        var prevCartItemsString = request.Cookies["Cart"];
        if (!string.IsNullOrEmpty(prevCartItemsString))
        {
            cartList = JsonConvert.DeserializeObject<List<CartViewModel>>(prevCartItemsString);
        }

        return cartList;
    }

    public List<ProductCartViewModel> GetProductsInCart(HttpRequest request)
    {
        var cartItems = GetCartItems(request);
        if (!cartItems.Any())
        {
            return null;
        }

        var cartItemProductIds = cartItems.Select(x => x.ProductId).ToList();
        var products = _context.Products
            .Where(p => cartItemProductIds.Contains(p.Id))
            .ToList();

        List<ProductCartViewModel> result = new List<ProductCartViewModel>();
        foreach (var item in products)
        {
            var newItem = new ProductCartViewModel
            {
                Id = item.Id,
                ImageName = item.ImageName,
                Price = item.Price - (item.Discount ?? 0),
                Title = item.Title,
                Count = cartItems.Single(x => x.ProductId == item.Id).Count,
                RowSumPrice = (item.Price - (item.Discount ?? 0)) * cartItems.Single(x => x.ProductId == item.Id).Count,
            };

            result.Add(newItem);
        }

        return result;
    }

    public IActionResult UpdateCart(CartViewModel request, HttpRequest httpRequest, HttpResponse httpResponse)
    {
        var product = _context.Products.FirstOrDefault(x => x.Id == request.ProductId);
        if (product == null)
        {
            return new NotFoundResult();
        }

        if (product.Qty < request.Count)
        {
            return new BadRequestResult();
        }

        var cartItems = GetCartItems(httpRequest);

        var foundProductInCart = cartItems.FirstOrDefault(x => x.ProductId == request.ProductId);
        if (foundProductInCart == null)
        {
            var newCartItem = new CartViewModel()
            {
                ProductId = request.ProductId,
                Count = request.Count
            };
            cartItems.Add(newCartItem);
        }
        else
        {
            if (request.Count > 0)
            {
                foundProductInCart.Count = request.Count;
            }
            else
            {
                cartItems.Remove(foundProductInCart);
            }
        }

        var json = JsonConvert.SerializeObject(cartItems);
        CookieOptions option = new CookieOptions();
        option.Expires = DateTime.Now.AddDays(7);
        httpResponse.Cookies.Append("Cart", json, option);

        var result = cartItems.Sum(x => x.Count);
        return new OkObjectResult(result);
    }
}
