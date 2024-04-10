using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingWebsite.Models
{
    public class PaymentInfo
    {
        public int Id { get; set; }

        [Required]
        public string CardholderName { get; set; }

        [Required]
        public string CreditCardNumber { get; set; } // This will store the encrypted credit card number

        [Column(TypeName = "decimal(18, 2)")]
        [Required]
        public decimal TotalPrice { get; set; } // Total price of the transaction

        [DataType(DataType.Date)]
        [Required]
        public DateTime Date { get; set; } // Date of the transaction

    }
}
