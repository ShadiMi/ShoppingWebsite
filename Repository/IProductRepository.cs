using ShoppingWebsite.Models; // Use your actual namespace here
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingWebsite.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int productId);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int productId);
    }
}
