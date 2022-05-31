using PlaylistES.Models;
using PlaylistES.Services;
using Microsoft.AspNetCore.Mvc;

namespace PlaylistES.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PlaylistsController : Controller
    {
        private readonly UserService _UserService;
        private readonly PlaylistsService _PlaylistService;
        private readonly YtVideoService _VideoService;

        public PlaylistsController(UserService userService, PlaylistsService playlistService, YtVideoService videoService)
        {
            _UserService = userService;
            _VideoService = videoService;
            _PlaylistService = playlistService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult>  MyPlaylists(string userId)
         {
            var user = await _UserService.GetOneIDAsync(userId);

            var data = await _PlaylistService.GetFromUserAsync(userId);


            ViewBag.user = user;

            return View(data);
        }


        public async Task<List<Playlists>> Get() =>
            await _PlaylistService.GetAsync();
        
        [HttpGet("{userId}")]
        public async Task<IActionResult> CreatePlaylist(string userId)
        {
            var user = await _UserService.GetOneIDAsync(userId);

            ViewBag.user = user;
            return View();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Playlists>> Get(string id)
        {
            var playlist = await _PlaylistService.GetOneAsync(id);

            if (playlist is null)
            {
                return NotFound();
            }

            return playlist;
        }


        [HttpPost("{userId}")]
        public async Task<IActionResult> CreatePlaylist([FromForm] Playlists newPlaylist)
        {
            newPlaylist.PlaylistId = null;
            newPlaylist.Videos.Clear();

            await _PlaylistService.CreateAsync(newPlaylist);

            await updateUsersPlaylists(newPlaylist.Creator_id);

            return Redirect("/Playlists/MyPlaylists/"+newPlaylist.Creator_id);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var videos = await _VideoService.GetAsync(id);
            var playlist = await _PlaylistService.GetOneAsync(id);

            foreach (var video in videos)
            {
                await _VideoService.RemoveAsync(video.id);

            }

            await _PlaylistService.RemoveAsync(id);

            await updateUsersPlaylists(playlist.Creator_id);


            return Redirect("/Playlists/MyPlaylists/"+playlist.Creator_id);
        }


        private async Task updateUsersPlaylists(string userId)
        {
            var playlists = await _PlaylistService.GetFromUserAsync(userId);
            var user = await _UserService.GetOneIDAsync(userId);
            user.Playlists.Clear();
            foreach (var playlist in playlists) {
                user.Playlists.Add(playlist.PlaylistId);
            }
            await _UserService.UpdateAsync(user.UserId, user);  
        }

    }
}

