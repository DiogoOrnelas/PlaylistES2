using Microsoft.AspNetCore.Mvc;
using PlaylistES.Models;
using PlaylistES.Services;
using System.Diagnostics;
using Microsoft.Extensions.Options;

namespace PlaylistES.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserService _UserService;

        public HomeController(ILogger<HomeController> logger, UserService userService)
        {
            _UserService = userService;
            _logger = logger;
        }


        public IActionResult Index()
        {
            ViewBag.Message= TempData["Message"];
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [HttpGet("{userID}")]
        public async Task<IActionResult> Home(string userID)
        {
            var user = await _UserService.GetOneIDAsync(userID);

            ViewBag.user = user;

            return View();
        }


        public IActionResult Register()
        {
            ViewBag.Message = TempData["Message"];
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> LoginUser([FromForm] string username, [FromForm] string password)
        {
            var user = await fetchUser(username, password);
            if (user is null)
            {
                TempData["Message"] = "Combination of username and password is not correct!";
                return Redirect("/Home/index");
            }
            return Redirect("/Home/Home/"+user.UserId);
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromForm] User newUser)
        {
            newUser.UserId = null;
            newUser.Playlists.Clear();
            var userAlreadyExists = await _UserService.checkUsernameAsync(newUser.username);
            if (userAlreadyExists is null)
            {
                await _UserService.CreateAsync(newUser);

                return Redirect("/Home/index");
            }
            else {
                TempData["Message"]="This Username already exists!";

                return Redirect("/Home/Register");
            }
        }


        private async Task<User> fetchUser(string username, string password)
        {
            var user = await _UserService.GetOneAsync(username, password);
            if (user is null)
            {
                return null;
            }

            return user;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
} 