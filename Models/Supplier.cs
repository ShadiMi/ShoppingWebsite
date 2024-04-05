using ShoppingWebsite.Models;
using System.ComponentModel.DataAnnotations;

public class Supplier
{
    [Key]
    public int SupplierID { get; set; }
    public string CompanyName { get; set; }
    // Other fields as needed
    public string ContactName { get; set; }
    // Navigation property
    public List<Product> Products { get; set; }
    public string Logo { get; set; }
}