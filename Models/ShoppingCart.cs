using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using ShoppingWebsite.Models;

namespace ShoppingWebsite.Models
{
    public class ShoppingCart
    {
        public List<Product> Products { get; set; } = new List<Product>();

        // Retrieves the shopping cart from the session, or creates one if it doesn't exist
        public static ShoppingCart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            string cartString = session.GetString("Cart");

            if (string.IsNullOrEmpty(cartString))
            {
                return new ShoppingCart();
            }
            else
            {
                return JsonSerializer.Deserialize<ShoppingCart>(cartString);
            }
        }

        // Saves the current state of the cart to the session
        public void SaveCart(HttpContext httpContext)
        {
            var session = httpContext.Session;
            session.SetString("Cart", JsonSerializer.Serialize(this));
        }

        // Adds a product to the cart
        public void AddProduct(Product product)
        {
            var existingProduct = Products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                // For simplicity, assuming each product is unique in the cart. Implement quantity logic if needed.
            }
            else
            {
                Products.Add(product);
            }
        }

        // Removes a product from the cart
        public void RemoveProduct(int productId)
        {
            Products.RemoveAll(p => p.Id == productId);
        }

        // Calculates the total cost of the shopping cart
        public decimal Total => Products.Sum(p => p.Price);
    }
}
