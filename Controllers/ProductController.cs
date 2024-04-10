using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingWebsite.Data;
using ShoppingWebsite.Models;
using ShoppingWebsite.ViewModels;

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
public async Task<IActionResult> Index(string category, bool onSale = false, decimal? minPrice = null, decimal? maxPrice = null, string format = null, string sortOrder = null)
{
    var productsQuery = _context.Products.Include(p => p.Category).Include(p => p.Supplier).AsQueryable();
            var categories = await _context.Categories.ToListAsync();

            ViewBag.Formats = new List<string> { "Hardcover", "Paperback", "eBook", "Audiobook" }; 
    ViewBag.CurrentSort = sortOrder;
    ViewBag.CategoryFilter = category;
    ViewBag.OnSaleFilter = onSale;
    ViewBag.MinPriceFilter = minPrice;
    ViewBag.MaxPriceFilter = maxPrice;
    ViewBag.FormatFilter = format;

    if (!string.IsNullOrEmpty(category))
    {
        productsQuery = productsQuery.Where(p => p.Category.CategoryName == category);
    }
    if (onSale)
    {
        productsQuery = productsQuery.Where(p => p.IsOnSale);
    }
    if (minPrice.HasValue)
    {
        productsQuery = productsQuery.Where(p => p.UnitPrice >= minPrice.Value);
    }
    if (maxPrice.HasValue)
    {
        productsQuery = productsQuery.Where(p => p.UnitPrice <= maxPrice.Value);
    }
    if (!string.IsNullOrEmpty(format))
    {
        productsQuery = productsQuery.Where(p => p.Format == format);
    }

            // Sorting logic based on sortOrder
            switch (sortOrder)
            {
                case "NameAsc":
                    productsQuery = productsQuery.OrderBy(p => p.ProductName);
                    break;
                case "NameDesc":
                    productsQuery = productsQuery.OrderByDescending(p => p.ProductName);
                    break;
                case "PriceAsc":
                    productsQuery = productsQuery.OrderBy(p => (int)p.UnitPrice);
                    break;
                case "PriceDesc":
                    productsQuery = productsQuery.OrderByDescending(p => (int)p.UnitPrice);
                    break;
                default:
                    // Default sorting criteria or no sorting
                    break;
            }

            var products = await productsQuery.ToListAsync();

    var cartCount = HttpContext.Session.GetInt32("CartCount") ?? 0;
    ViewBag.CartCount = cartCount;
            // Before the return View(products); line, add:
    ViewBag.Categories = await _context.Categories.OrderBy(c => c.CategoryName).Select(c => new SelectListItem{Value = c.CategoryName,Text = c.CategoryName}).ToListAsync();
            ViewBag.CategoryFilter = category;
            return View(products);
}

        // GET: Products/Create
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName");
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "CompanyName");
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("ProductID,ProductName,SupplierID,CategoryID,UnitPrice,UnitsInStock,UnitsOnOrder,Image,IsOnSale,SalePrice,Format")] Product Model)
        {
            if (ModelState.IsValid==false)
            {
                _context.Add(Model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName", Model.CategoryID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "CompanyName", Model.SupplierID);
            return View(Model);
        }

        // GET: Products/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Model = await _context.Products.FindAsync(id);
            if (Model == null)
            {
                return NotFound();
            }

            // Populate dropdown lists
            ViewData["CategoryID"] = new SelectList(await _context.Categories.ToListAsync(), "CategoryID", "CategoryName", Model.CategoryID);
            ViewData["SupplierID"] = new SelectList(await _context.Suppliers.ToListAsync(), "SupplierID", "CompanyName", Model.SupplierID);
            return View(Model);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,ProductName,SupplierID,CategoryID,UnitPrice,UnitsInStock,UnitsOnOrder,Image,IsOnSale,SalePrice,Format")] Product Model)
        {
            if (id != Model.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid == false)
            {
                try
                {
                    _context.Update(Model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(Model.ProductID))
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

            ViewData["CategoryID"] = new SelectList(await _context.Categories.ToListAsync(), "CategoryID", "CategoryName", Model.CategoryID);
            ViewData["SupplierID"] = new SelectList(await _context.Suppliers.ToListAsync(), "SupplierID", "CompanyName", Model.SupplierID);
            return View(Model);
        }



        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }
        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Model = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (Model == null)
            {
                return NotFound();
            }

            return View(Model);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Model = await _context.Products.FindAsync(id);
            _context.Products.Remove(Model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Model = await _context.Products
                .Include(p => p.Category) 
                .Include(p => p.Supplier) 
                .FirstOrDefaultAsync(m => m.ProductID == id);

            if (Model == null)
            {
                return NotFound();
            }

            return View(Model);
        }

        public async Task<IActionResult> ProductsByCategory(int categoryId, string sortOrder)
        {
            var categoryName = await _context.Categories
                .Where(c => c.CategoryID == categoryId)
                .Select(c => c.CategoryName)
                .FirstOrDefaultAsync();
            var categories = await _context.Categories.ToListAsync();
            ViewData["CategoryId"] = categoryId;
            if (string.IsNullOrEmpty(categoryName))
            {
                return NotFound("Category not found.");
            }

            ViewData["CategoryName"] = categoryName;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["PriceSortParam"] = string.IsNullOrEmpty(sortOrder) ? "price_desc" : "";

            // Fetch products including the supplier
            var products = await _context.Products
                .Include(p => p.Supplier)
                .Where(p => p.CategoryID == categoryId)
                .ToListAsync(); // Materialize the query here

            // Apply sorting in memory
            products = sortOrder switch
            {
                "price_desc" => products.OrderByDescending(p => p.UnitPrice).ToList(),
                _ => products.OrderBy(p => p.UnitPrice).ToList(),
            };

            return View(products);
        }

        public async Task<IActionResult> NotifyWhenAvailable(int productId)
        {
            var Model = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == productId);
            if (Model != null)
            {
                Model.UnitsOnOrder += 1;
                _context.Update(Model);
                await _context.SaveChangesAsync();

                TempData["Notification"] = "You will be notified when the Model is back in stock.";
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Orders()
        {
            var productsWithOrders = await _context.Products
                .Where(p => p.UnitsOnOrder > 0)
                .Select(p => new OrderViewModel
                {
                    ProductId = p.ProductID,
                    ProductName = p.ProductName,
                    UnitsOnOrder = p.UnitsOnOrder
                })
                .ToListAsync();

            return View(productsWithOrders);
        }


        [HttpPost]
        public async Task<IActionResult> Resupply(int productId, int resupplyAmount)
        {
            var Model = await _context.Products.FindAsync(productId);
            if (Model == null) return NotFound();

            // Logic for resupplying the Model
            Model.UnitsInStock += resupplyAmount;
            Model.UnitsOnOrder = 0;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Orders));
        }


        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return RedirectToAction(nameof(Index));
            }

            var searchResults = await _context.Products
                    .Where(p => EF.Functions.Like(p.ProductName, $"%{query}%") || EF.Functions.Like(p.Category.CategoryName, $"%{query}%"))
                    .Include(p => p.Category)
                    .ToListAsync();

            ViewData["SearchQuery"] = query;

            return View(searchResults);
        }
    }
}
