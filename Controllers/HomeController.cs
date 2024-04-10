using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using ShoppingWebsite.Data;
using ShoppingWebsite.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var deals = await _context.Products
                           .Where(p => p.IsOnSale)
                           .Include(p => p.Category) // Assuming you have a Category navigation property
                           .Include(p => p.Supplier) // Assuming you have a Supplier navigation property
                           .ToListAsync();

            var categories = await _context.Categories.ToListAsync();

            ViewBag.Deals = deals;
            ViewBag.Categories = categories;

            var cartCount = HttpContext.Session.GetInt32("CartCount") ?? 0;

            // Pass cart count to view via ViewBag
            ViewBag.CartCount = cartCount;

            return View();
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
            {
                var productsWithOrders = _context.Products.Count(p => p.UnitsOnOrder > 0);
                ViewBag.ProductsWithOrdersCount = productsWithOrders;
            }
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}
