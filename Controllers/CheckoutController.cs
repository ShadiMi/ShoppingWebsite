using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ShoppingWebsite.Data;
using ShoppingWebsite.Models;
using ShoppingWebsite.ViewModels;
using ShoppingWebsite.Views.Utilities;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShoppingWebsite.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
    


        public CheckoutController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public CheckoutViewModel ConvertCartToCheckoutViewModel(ShoppingCart cart)
        {
            var checkoutViewModel = new CheckoutViewModel
            {
                Items = cart.CartItems.Select(item => new CartItemViewModel
                {
                    ProductId = item.ProductId,
                    ProductName = _context.Products.FirstOrDefault(p => p.ProductID == item.ProductId)?.ProductName ?? "Unknown Product",
                    Price = _context.Products.FirstOrDefault(p => p.ProductID == item.ProductId)?.UnitPrice ?? 0,
                    Quantity = item.Quantity
                }).ToList(),
                TotalCost = cart.CartItems.Sum(item => (_context.Products.FirstOrDefault(p => p.ProductID == item.ProductId)?.UnitPrice ?? 0) * item.Quantity)
            };

            return checkoutViewModel;
        }


        private ShoppingCart GetCart()
        {
            var cartString = HttpContext.Session.GetString("Cart");
            return string.IsNullOrEmpty(cartString) ? new ShoppingCart() : JsonSerializer.Deserialize<ShoppingCart>(cartString);
        }

        public IActionResult Index()
        {
            var cart = GetCart(); // Retrieve the current shopping cart.
            var checkoutViewModel = ConvertCartToCheckoutViewModel(cart); // Convert the cart into a CheckoutViewModel.

            return View(checkoutViewModel); // Pass the populated CheckoutViewModel to the view.
        }




        [HttpPost]
        public async Task<IActionResult> ProcessPayment(CheckoutViewModel model)
        {
            if (ModelState.IsValid==false)
            {
                // Assuming you retrieve the cart again as it might have been updated
                var cart = GetCart();

                // Process payment here...
                // For simplicity, let's assume the payment is always successful

                foreach (var item in cart.CartItems)
                {
                    var Model = await _context.Products.FindAsync(item.ProductId);
                    if (Model != null)
                    {
                        Model.UnitsInStock -= item.Quantity;
                    }
                }
                await _context.SaveChangesAsync();

                // Clear the cart after successful payment
                ClearCart();

                TempData["PaymentResult"] = "Payment successful. Your order has been placed!";
                return RedirectToAction("PaymentSuccess");
            }
            else
            {
                return View("Index", model);
            }
        }

        private void ClearCart()
        {
            // Implementation of how to clear the shopping cart after successful payment
            HttpContext.Session.Remove("Cart"); 
            HttpContext.Session.SetInt32("CartCount", 0);    
        }
        public IActionResult PaymentSuccess()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddPaymentInfo()
        {
            var viewModel = new PaymentInfoViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddPaymentInfo(PaymentInfoViewModel model)
        {
            if (ModelState.IsValid==false)
            {
                // Assuming the payment is processed and successful

                var cart = GetCart(); // Retrieve the cart

                foreach (var item in cart.CartItems)
                {
                    var Model = await _context.Products.FindAsync(item.ProductId);
                    if (Model != null)
                    {
                        // Check stock availability
                        if (Model.UnitsInStock >= item.Quantity)
                        {
                            Model.UnitsInStock -= item.Quantity; // Deduct the purchased quantity from stock
                        }
                       
                    }
                }

                // Encrypt the credit card information (as a simulation here)
                var encryptedCreditCardNumber = AesEncryptionHelper.Encrypt(model.CreditCardNumber, "12345678901234567890123456789012", "1234567890123456");

                var paymentInfo = new PaymentInfo
                {
                    CardholderName = model.CardholderName,
                    CreditCardNumber = encryptedCreditCardNumber,
                    TotalPrice = cart.CartItems.Sum(item => item.Price * item.Quantity), // Assuming you have price in CartItem
                    Date = DateTime.Now
                };

                _context.PaymentInfos.Add(paymentInfo);
                await _context.SaveChangesAsync();

                ClearCart(); // Clear the cart after successful payment and inventory update

                return RedirectToAction("PaymentSuccess");
            }

            // If we get here, something was wrong with the ModelState
            return View(model);
        }

        public async Task<IActionResult> Checkout()
        {
            var cart = GetCart();
            var checkoutViewModel = ConvertCartToCheckoutViewModel(cart);

            return View("Checkout", checkoutViewModel);
        }
    }
}
