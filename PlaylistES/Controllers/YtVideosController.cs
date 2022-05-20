using PlaylistES.Models;
using PlaylistES.Services;
using Microsoft.AspNetCore.Mvc;

namespace PlaylistES.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class YtVideosController : Controller
    {
        
        private readonly YtVideoService _VideoService;
        private readonly PlaylistsService _PlaylistService;


        public YtVideosController(YtVideoService videoService, PlaylistsService playlistService)
        {
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


        [HttpGet("{id:length(24)}")]
        [Route("Videos/YTVideos/{id:length(24)}")]
        public async Task<IActionResult> Videos(string id)
        {
            var playlist = await _PlaylistService.GetOneAsync(id);
            var video = await _VideoService.GetAsync(id);
            if (video is null)
            {
                return NotFound();
            }
            ViewBag.Playlist = playlist;

            return View(video);
        }

        public async Task<IActionResult> Add(string id)
        {
            var playlist = await _PlaylistService.GetOneAsync(id);

            return View();
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

        [HttpPost]
        public async Task<IActionResult> Post(YouTubeVideo newVideo)
        {
            await _VideoService.CreateAsync(newVideo);

            return CreatedAtAction(nameof(Get), new { id = newVideo.id }, newVideo);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, YouTubeVideo updatedVideo)
        {
            var user = await _VideoService.GetOneAsync(id);

            if (user is null)
            {
                return NotFound();
            }

            updatedVideo.id = user.id;

            await _VideoService.UpdateAsync(id, updatedVideo);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var video = await _VideoService.GetAsync(id);

            if (video is null)
            {
                return NotFound();
            }

            await _VideoService.RemoveAsync(id);

            return NoContent();
        }
    }
}

