using Microsoft.AspNetCore.Mvc;
using ShoppingWebsite.Data;
using ShoppingWebsite.Models;
using ShoppingWebsite.ViewModels;
using System.Text.Json;


public class ShoppingCartController : Controller
{
    private readonly ApplicationDbContext _context;

    public ShoppingCartController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var cart = GetCart();
        var model = new ShoppingCartViewModel();

        foreach (var cartItem in cart.CartItems)
        {
            var Model = await _context.Products.FindAsync(cartItem.ProductId);
            if (Model != null)
            {
                model.Items.Add(new CartItemViewModel
                {
                    ProductId = Model.ProductID,
                    ProductName = Model.ProductName,
                    Price = Model.IsOnSale ? Model.SalePrice : Model.UnitPrice,
                    ImageUrl = Model.Image,
                    Quantity = cartItem.Quantity
                });
            }
        }
        var cartCount = HttpContext.Session.GetInt32("CartCount") ?? 0;

        // Pass cart count to view via ViewBag
        ViewBag.CartCount = cartCount;

        return View(model);
    }


    public async Task<IActionResult> AddToCart(int id)
    {
        // Assuming 'id' is ProductId
        var cart = GetCart();
        cart.AddProduct(id, 1); // Add one quantity of the Model
        SaveCart(cart);
        
        var count = cart.CartItems.Sum(item => item.Quantity); // Assuming CartItems is a List<CartItem>
        HttpContext.Session.SetInt32("CartCount", count);

        return RedirectToAction("Index", "Products");

    }

    private ShoppingCart GetCart()
    {
        var cartString = HttpContext.Session.GetString("Cart");
        return string.IsNullOrEmpty(cartString) ? new ShoppingCart() : JsonSerializer.Deserialize<ShoppingCart>(cartString);
    }

    private void SaveCart(ShoppingCart cart)
    {
        var cartString = JsonSerializer.Serialize(cart);
        HttpContext.Session.SetString("Cart", cartString);
    }
    public IActionResult RemoveFromCart(int productId)
    {
        var cart = GetCart();

        cart.RemoveFromCart(productId);
        SaveCart(cart);

        
        var count = cart.CartItems.Sum(item => item.Quantity); // Assuming CartItems is a List<CartItem>
        HttpContext.Session.SetInt32("CartCount", count);

        return RedirectToAction("Index");
  
    }
    [HttpPost]
    public async Task<IActionResult> BuyNow(int id)
    {
        // Assuming 'id' is ProductId and GetCart() retrieves the current shopping cart
        var cart = GetCart();

        // Add the Model to the cart
        cart.AddProduct(id, 1); // Add one quantity of the Model
        SaveCart(cart);

        var count = cart.CartItems.Sum(item => item.Quantity);
        HttpContext.Session.SetInt32("CartCount", count);

        // Redirect to the Checkout page
        return RedirectToAction("Index", "Checkout");
    }





}