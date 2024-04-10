using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ShoppingWebsite.Models
{
    public class ShoppingCart
    {
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();

        public void AddProduct(int productId, int quantity)
        {
            var cartItem = CartItems.FirstOrDefault(c => c.ProductId == productId);
            if (cartItem != null)
            {
                // If the item is already in the cart, increase the quantity
                cartItem.Quantity += quantity;
            }
            else
            {
                // Add the new item to the cart
                CartItems.Add(new CartItem { ProductId = productId, Quantity = quantity });
            }
        }

        public void RemoveFromCart(int productId)
        {
            var existingItem = CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (existingItem != null && existingItem.Quantity > 1)
            {
                // If more than one, decrease the quantity
                existingItem.Quantity--;
            }
            else
            {
                // If only one item or not concerned about quantity, remove it completely
                CartItems.RemoveAll(ci => ci.ProductId == productId);
            }
        }
    }

    
}
