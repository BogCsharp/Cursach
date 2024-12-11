using Course2.Models;
using Course2.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;


namespace Course2.Controllers
{
    public class HomeController : Controller
    {
        UserContext db;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, UserContext contextdb)
        {
            db = contextdb;

            _logger = logger;
        }
       
        public IActionResult Index()
        {
            return View();

        }
        public IActionResult Privacy()
        {
            return View();
        }


    }
}

