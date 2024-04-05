using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingWebsite.Data;
using ShoppingWebsite.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShoppingWebsite.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        // Updated to include sorting logic in a single Index method
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["CategorySortParm"] = sortOrder == "Category" ? "category_desc" : "Category";
            ViewData["StockSortParm"] = sortOrder == "Stock" ? "stock_desc" : "Stock";

            var products = from p in _context.Products.Include(p => p.Category).Include(p => p.Supplier)
                           select p;

            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(p => p.ProductName);
                    break;
                case "Category":
                    products = products.OrderBy(p => p.Category.CategoryName);
                    break;
                case "category_desc":
                    products = products.OrderByDescending(p => p.Category.CategoryName);
                    break;
                case "Stock":
                    products = products.OrderBy(p => p.UnitsInStock);
                    break;
                case "stock_desc":
                    products = products.OrderByDescending(p => p.UnitsInStock);
                    break;
                default:
                    products = products.OrderBy(p => p.ProductName);
                    break;
            }

            return View(await products.ToListAsync());
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName");
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "CompanyName");
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,ProductName,SupplierID,CategoryID,UnitPrice,UnitsInStock,UnitsOnOrder,Image")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName", product.CategoryID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "CompanyName", product.SupplierID);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName", product.CategoryID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "CompanyName", product.SupplierID);
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,ProductName,SupplierID,CategoryID,UnitPrice,UnitsInStock,UnitsOnOrder,Image")] Product product)
        {
            if (id != product.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName", product.CategoryID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "CompanyName", product.SupplierID);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }
    }
}
