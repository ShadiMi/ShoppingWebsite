using ShoppingWebsite.Models;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingWebsite.Repository
{
    public class ProductRepository : IProductRepository
    {
        // This is a simple in-memory list to simulate database data.
        // In a real application, you would interact with a database here.
        private List<Product> products = new List<Product>
        {
            new Product { Id = 1, Name = "Product 1", Price = 100m, Description = "Description 1", Category = "Category 1", ImageUrl = "image1.jpg", StockQuantity = 10 },
            new Product { Id = 2, Name = "Product 2", Price = 200m, Description = "Description 2", Category = "Category 2", ImageUrl = "image2.jpg", StockQuantity = 0 }, // Example of unavailable product
            // Add more products as needed for demonstration
        };

        public IEnumerable<Product> GetAllProducts()
        {
            return products;
        }

        public Product GetProductById(int productId)
        {
            return products.FirstOrDefault(p => p.Id == productId);
        }

        public void AddProduct(Product product)
        {
            // Assuming Ids are auto-incremented. In a database, this would be handled by the DBMS.
            product.Id = products.Max(p => p.Id) + 1;
            products.Add(product);
        }

        public void UpdateProduct(Product product)
        {
            var index = products.FindIndex(p => p.Id == product.Id);
            if (index != -1)
            {
                products[index] = product;
            }
        }

        public void DeleteProduct(int productId)
        {
            var product = GetProductById(productId);
            if (product != null)
            {
                products.Remove(product);
            }
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return products.Where(p => p.Category == category);
        }

        public IEnumerable<Product> SearchProducts(string searchTerm)
        {
            return products.Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm));
        }

        public IEnumerable<Product> GetAvailableProducts()
        {
            return products.Where(p => p.IsAvailable);
        }
    }
}
