using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingWebsite.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required]
        public string ProductName { get; set; }

        // Adding [Required] to ensure a Product must have a Supplier and Category
        [Required]
        public int? SupplierID { get; set; }
        public Supplier Supplier { get; set; }

        [Required]
        public int? CategoryID { get; set; }
        public Category Category { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; } = 0;

        public int UnitsInStock { get; set; } = 0;
        public int UnitsOnOrder { get; set; } = 0;

        public string Image { get; set; }
        public bool IsOnSale { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal SalePrice { get; set; }

        public string? Format { get; set; } // Making Format nullable if using C# 8.0 nullable reference types
     }
}
