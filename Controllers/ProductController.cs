using Microsoft.AspNetCore.Mvc;
using ShoppingWebsite.Models;
using ShoppingWebsite.Repository;
using System.Linq;

namespace ShoppingWebsite.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            var products = _productRepository.GetAllProducts();
            return View(products);
        }

        public IActionResult Available()
        {
            var products = _productRepository.GetAvailableProducts();
            return View("Index", products); // Reuse the Index view for displaying available products
        }

        public IActionResult ByCategory(string category)
        {
            var products = _productRepository.GetProductsByCategory(category);
            return View("Index", products); // Reuse the Index view for displaying products by category
        }

        public IActionResult Search(string searchTerm)
        {
            var products = _productRepository.SearchProducts(searchTerm);

            // If there's only one result, you might want to redirect to a detailed view of that product.
            if (products.Count() == 1)
            {
                return RedirectToAction("Detail", new { id = products.Single().Id });
            }

            return View("Index", products); // Reuse the Index view for displaying search results
        }

        // Example Detail action, assuming you have one
        public IActionResult Detail(int id)
        {
            var product = _productRepository.GetProductById(id);
            if (product == null) return NotFound();
            return View(product);
        }
    }
}
