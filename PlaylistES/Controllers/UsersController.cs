using PlaylistES.Models;
using PlaylistES.Services;
using Microsoft.AspNetCore.Mvc;

namespace PlaylistES.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UsersController : Controller
    {

        private readonly UserService _UserService;

        public UsersController(UserService userService) =>
            _UserService = userService;

        [HttpGet]
        public async Task<List<User>> Get() =>
            await _UserService.GetAsync();

        //IS THIS CONTROLLER NEEDED???????????? NOPE.

        [HttpPost]

        public async Task<IActionResult> Post(User newUser)
        {
            await _UserService.CreateAsync(newUser);

            return CreatedAtAction(nameof(Get), new { id = newUser.UserId }, newUser);
        }
    }
}

