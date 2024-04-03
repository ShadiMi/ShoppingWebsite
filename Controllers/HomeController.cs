using Microsoft.AspNetCore.Mvc;

namespace ShoppingWebsite.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
