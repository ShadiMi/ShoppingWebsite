using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ShoppingWebsite.ViewModels
{
    public class PaymentInfoViewModel
    {
        [Required]
        [DisplayName("Cardholder's Name")]
        public string CardholderName { get; set; }

        [Required]
        [CreditCard]
        [DisplayName("Credit Card Number")]
        public string CreditCardNumber { get; set; }

        [Required]
        [RegularExpression(@"\d{2}/\d{2}", ErrorMessage = "Invalid expiration date. Format MM/YY")]
        [DisplayName("Expiration Date")]
        public string ExpirationDate { get; set; }

        [Required]
        [RegularExpression(@"\d{3}", ErrorMessage = "Invalid CVV")]
        public string CVV { get; set; }

        [Required]
        [DisplayName("Total Price")]
        [DataType(DataType.Currency)]
        public decimal TotalPrice { get; set; }

        [Required]
        [DisplayName("Transaction Date")]
        [DataType(DataType.Date)]
        public DateTime TransactionDate { get; set; }
    }
}
