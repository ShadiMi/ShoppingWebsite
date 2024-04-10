using Microsoft.AspNetCore.Mvc;
using ShoppingWebsite.Data;

public class CustomersController : Controller
{
    private readonly ApplicationDbContext _context;

    public CustomersController(ApplicationDbContext context)
    {
        _context = context;
    }

}
