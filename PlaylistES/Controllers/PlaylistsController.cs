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
            Console.WriteLine("Hello there! this is playlist: "+ newPlaylist.PlaylistName );
            newPlaylist.PlaylistId = null;
            newPlaylist.Videos.Clear();
            var user = await _UserService.GetOneIDAsync(newPlaylist.Creator_id);
            //TODO: ALTER THIS ID?
            user.Playlists.Add(newPlaylist.PlaylistId);
            await _PlaylistService.CreateAsync(newPlaylist);
            await _UserService.UpdateAsync(user.UserId, user);
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
            Console.WriteLine("playlist ID: " + playlist.PlaylistId);
            Console.WriteLine("CREATOR ID: " + playlist.Creator_id);
            var user = await _UserService.GetOneIDAsync(playlist.Creator_id);

            user.Playlists.Remove(id);
            await _UserService.UpdateAsync(user.UserId, user);
            await _PlaylistService.RemoveAsync(id);

            return Redirect("/Playlists/MyPlaylists/"+playlist.Creator_id);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Playlists updatedPlaylist)
        {
            var playlist = await _PlaylistService.GetOneAsync(id);

            if (playlist is null)
            {
                return NotFound();
            }

            updatedPlaylist.PlaylistId = playlist.PlaylistId;

            await _PlaylistService.UpdateAsync(id, updatedPlaylist);

            return NoContent();
        }

        
    }
}

