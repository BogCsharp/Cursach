using Course3.Data;
using Course3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Course3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _db = context;
            _logger = logger;
        }

        public IActionResult Index()

        {
           
            var catalog = GetCatalog();
            return View(catalog);
        }

        private List<Catalog> GetCatalog()
        {
            
            return _db.Catalogs.ToList();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Catalog catalog)
        {
            if (ModelState.IsValid)
            {
                _db.Catalogs.Add(catalog);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(catalog);
        }
        [Authorize(Roles = "Admin")]

        public IActionResult EditCatalog(int id)
        {
            var catalog = _db.Catalogs.Find(id);
            if (catalog == null)
            {
                return NotFound();
            }
            return View(catalog);
        }


        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCatalog(int id, Catalog catalog)
        {
            if (id != catalog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.Update(catalog);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(catalog);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DeleteCatalog(int id)
        {
            var catalog = _db.Catalogs.Find(id);
            if (catalog == null)
            {
                return NotFound();
            }
            return View(catalog);
        }

     
        [HttpPost, ActionName("DeleteCatalog")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCatalogConfirmed(int id)
        {
            var catalog = _db.Catalogs.Find(id);
            if (catalog != null)
            {
                _db.Catalogs.Remove(catalog);
                _db.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }


            [Authorize(Roles = "Admin")]
        public IActionResult Privacy()
        {
            
            var products = _db.Products.ToList();
            return PartialView("Privacy", products);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
             [Authorize(Roles = "Admin")]
        public IActionResult AllOrders()
        {
            var orders = _db.Orders.ToList(); 
            return View(orders); 
        }
    }
}
