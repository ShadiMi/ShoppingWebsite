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

        // Foreign keys and navigation properties for Category and Supplier
        public int? SupplierID { get; set; }
        public Supplier Supplier { get; set; }

        public int? CategoryID { get; set; }
        public Category Category { get; set; }


        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; } = 0;

        public int UnitsInStock { get; set; } = 0;
        public int UnitsOnOrder { get; set; } = 0;

        public string Image { get; set; }


    }
}
