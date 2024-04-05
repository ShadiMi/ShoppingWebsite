using Microsoft.AspNetCore.Mvc;
using ShoppingWebsite.Services; // Adjust namespace to where your services are defined
using System.Threading.Tasks;

namespace ShoppingWebsite.Controllers
{
    public class CustomersController : Controller
    {
        private readonly CustomerService _customerService;

        public CustomersController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<IActionResult> Index()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return View(customers);
        }

        // Additional actions (Create, Details, Edit, Delete) here...
    }
}
