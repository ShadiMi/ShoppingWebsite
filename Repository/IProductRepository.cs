using ShoppingWebsite.Models;
using ShoppingWebsite.Models;
using System.Collections.Generic;

namespace ShoppingWebsite.Repository
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAllProducts();
        Product GetProductById(int productId);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int productId);

        // New method signatures based on updated Product properties
        IEnumerable<Product> GetProductsByCategory(string category);
        IEnumerable<Product> SearchProducts(string searchTerm);
        IEnumerable<Product> GetAvailableProducts();
    }
}
