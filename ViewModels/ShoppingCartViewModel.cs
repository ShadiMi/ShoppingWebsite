using ShoppingWebsite.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingWebsite.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();
        public decimal Total => Items.Sum(item => item.Price * item.Quantity);
    }
}
