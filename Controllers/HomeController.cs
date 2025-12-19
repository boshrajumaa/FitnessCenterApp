using FitnessCenterApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FitnessCenterApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult ApiReport()
        {
            return View();
        }
        public IActionResult Index()
        {
            Console.WriteLine($"User visited Home Page at {DateTime.Now}"); 
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}