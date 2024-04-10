using System.ComponentModel.DataAnnotations;

namespace ShoppingWebsite.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string Image { get; set; } // Assuming this stores the URL to the image

        // Navigation property for related products
        public List<Product> Products { get; set; }
    }
}
