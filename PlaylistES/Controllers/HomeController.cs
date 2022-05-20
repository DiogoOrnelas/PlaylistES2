using Microsoft.AspNetCore.Mvc;
using PlaylistES.Models;
using PlaylistES.Services;
using System.Diagnostics;
using Microsoft.Extensions.Options;

namespace PlaylistES.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
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

        public IActionResult Home()
        {
            
            return View();
        }
        public IActionResult Register()
        {

            return View();
        }
        public IActionResult Watch()
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