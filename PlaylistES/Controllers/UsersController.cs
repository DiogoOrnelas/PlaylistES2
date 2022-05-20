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


        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<User>> Get(string id)
        {
            var user = await _UserService.GetAsync(id);

            if (user is null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost]

        public async Task<IActionResult> Post(User newUser)
        {
            await _UserService.CreateAsync(newUser);

            return CreatedAtAction(nameof(Get), new { id = newUser.UserId }, newUser);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, User updatedUser)
        {
            var user = await _UserService.GetAsync(id);

            if (user is null)
            {
                return NotFound();
            }

            updatedUser.UserId = user.UserId;

            await _UserService.UpdateAsync(id, updatedUser);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _UserService.GetAsync(id);

            if (user is null)
            {
                return NotFound();
            }

            await _UserService.RemoveAsync(id);

            return NoContent();
        }
    }
}

