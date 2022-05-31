using PlaylistES.Models;
using PlaylistES.Services;
using Microsoft.AspNetCore.Mvc;

namespace PlaylistES.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class YtVideosController : Controller
    {

        private readonly UserService _UserService;
        private readonly YtVideoService _VideoService;
        private readonly PlaylistsService _PlaylistService;


        public YtVideosController(UserService userService, YtVideoService videoService, PlaylistsService playlistService)
        {
            _UserService = userService;
            _VideoService = videoService;
            _PlaylistService = playlistService;
        }

        [HttpGet]
        public async Task<IActionResult> Videos()
        {
            var data = await _VideoService.GetAsync();
            return View(data);
        }

        public async Task<List<YouTubeVideo>> Get() =>
            await _VideoService.GetAsync();


        [HttpGet("{id}")]
        public async Task<IActionResult> Videos(string id)
        {

            var playlist = await _PlaylistService.GetOneAsync(id);
            var video = await _VideoService.GetAsync(id);
            var user = await _UserService.GetOneIDAsync(playlist.Creator_id);
            if (video is null)
            {
                return NotFound();
            }
            ViewBag.Playlist = playlist;
            ViewBag.user = user;
            return View(video);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Watch(string id, [FromQuery(Name = "watch")] string Watch)
        {
            var playlist = await _PlaylistService.GetOneAsync(id);
            var video = await _VideoService.GetAsync(id);
            var user = await _UserService.GetOneIDAsync(playlist.Creator_id);

            var watch = await _VideoService.GetOneAsync(Watch);
            if (video is null)
            {
                return NotFound();
            }
            if (video.Count > 0) { 
                if (Watch == "Default") {
                    ViewBag.Watch = video[0];
                }
                else {
                    ViewBag.Watch = watch;
                }
            }
            ViewBag.Playlist = playlist;
            ViewBag.User = user;

            return View(video);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Delete(string id, [FromQuery(Name = "vidId")] string videoId)
        {
            var video = await _VideoService.GetOneAsync(videoId);
            if (video is null)
            {
                return NotFound();
            }
            var playlist = await _PlaylistService.GetOneAsync(id);
            playlist.Videos.Remove(video.id);
            await _PlaylistService.UpdateAsync(id, playlist);
            await _VideoService.RemoveAsync(videoId);

            return Redirect("/YTVideos/Videos/" + id);
        }

        public async Task<ActionResult<YouTubeVideo>> Get(string id)
        {
            var video = await _VideoService.GetOneAsync(id);

            if (video is null)
            {
                return NotFound();
            }

            return video;
        }


        
    }
}

