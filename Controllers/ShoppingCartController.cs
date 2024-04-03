using Microsoft.AspNetCore.Mvc;
using ShoppingWebsite.Models;

namespace ShoppingWebsite.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ShoppingCart _shoppingCart;

        public ShoppingCartController(ShoppingCart shoppingCart)
        {
            _shoppingCart = shoppingCart;
        }

        public IActionResult Index()
        {
            return View(_shoppingCart);
        }

        // Methods to add or remove products, clear cart, etc.
    }
}
