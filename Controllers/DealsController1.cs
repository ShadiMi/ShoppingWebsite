using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingWebsite.Data; 

public class DealsController : Controller
{
    private readonly ApplicationDbContext _context;

    public DealsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Deals
    public async Task<IActionResult> Index()
    {
        var deals = await _context.Products
                 .Where(p => p.IsOnSale)
                 .Include(p => p.Category) // Include Category
                 .Include(p => p.Supplier) // Include Supplier
                 .ToListAsync();
        var cartCount = HttpContext.Session.GetInt32("CartCount") ?? 0;

        // Pass cart count to view via ViewBag
        ViewBag.CartCount = cartCount;

        return View(deals);
    }
    public IActionResult Deals()
    {
        var deals = _context.Products.Where(p => p.IsOnSale).ToList(); // Assuming IsOnSale is a bool indicating if the Model is on a deal
        return View(deals);
    }
}
