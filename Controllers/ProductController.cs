using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ETicaret.Data;
using ETicaret.Models;
using System.Linq;


namespace ETicaret.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }
       public IActionResult Category(int id, string sortOrder)
        {
            var products = _context.Products
                .Where(p => p.CategoryId == id)
                .ToList();

            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            ViewBag.CategoryName = category?.Name ?? "Kategori";
            ViewBag.CurrentSort = sortOrder;

            products = sortOrder switch
            {
                "price_asc" => products.OrderBy(p => p.Price).ToList(),
                "price_desc" => products.OrderByDescending(p => p.Price).ToList(),
                "name_asc" => products.OrderBy(p => p.Name).ToList(),
                "name_desc" => products.OrderByDescending(p => p.Name).ToList(),
                _ => products
            };

            return View("Category", products);
        }
        public IActionResult Details(int id)
        {
            var product = _context.Products
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefault(p => p.Id == id);

            if (product == null) return NotFound();

            return View(product);
        }
        public IActionResult Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return RedirectToAction("Index", "Home");

            var results = _context.Products
                .Where(p => p.Name.Contains(query))
                .ToList();

            ViewBag.Query = query;
            return View(results);
        }
    }
}
