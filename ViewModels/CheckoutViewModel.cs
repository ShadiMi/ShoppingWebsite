using System.Collections.Generic;

namespace ShoppingWebsite.ViewModels
{
    public class CheckoutViewModel
    {
        public string CardNumber { get; set; }
        public int ExpMonth { get; set; }
        public int ExpYear { get; set; }
        public string Cvc { get; set; }
        public PaymentInfoViewModel PaymentInfo { get; set; }
        public List<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();
        public decimal TotalCost { get; set; }

    }
}
