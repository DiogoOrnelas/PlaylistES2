using PlaylistES.Models;
using PlaylistES.Services;
using Microsoft.AspNetCore.Mvc;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;

namespace PlaylistES.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class SearchController : Controller
    {
        
        private readonly YtVideoService _VideoService;
        private readonly PlaylistsService _PlaylistService;


        public SearchController(YtVideoService videoService, PlaylistsService playlistService)
        {
            _VideoService = videoService;
            _PlaylistService = playlistService;
        }

        /*[HttpGet]
        public async Task<IActionResult> Search()
        {
            var data = await _VideoService.GetAsync();
            return View(data);
        }*/

        [HttpGet("{id:length(24)}")]
        [Route("Search/Search/{id:length(24)}")]
        public async Task<IActionResult> Search(string id)
        {
            ICollection<YouTubeVideo> allVideos = new List<YouTubeVideo>();
            var playlist = await _PlaylistService.GetOneAsync(id);
            string urlQuery = HttpContext.Request.QueryString.ToString();

            if (urlQuery.Length > 3)
            {
                string searchKeyword = urlQuery.Substring(3);
                Console.WriteLine("Looking for: " + searchKeyword + "\n");

                foreach (var video in await fetchVideos(searchKeyword))
                {
                    Console.WriteLine("Converting...\n");
                    allVideos.Add(await setupVideo(id, video));
                    Console.WriteLine("converted!!\n");
                }
                Console.WriteLine("ALL SET!");
            }
            else {
                Console.WriteLine("Not looking for anything yet....\n");
            }
            

            ViewBag.Playlist = playlist;

            return View(allVideos);
        }

        /*public async Task<IActionResult> Add(string id)
        {
            var playlist = await _PlaylistService.GetOneAsync(id);

            return View();
        }*/
        
        
       /* public async Task<ActionResult<YouTubeVideo>> Get(string id)
        {
            var video = await _VideoService.GetOneAsync(id);

            if (video is null)
            {
                return NotFound();
            }

            return video;
        */



        [HttpPost]
        [Route("Search/Search/Search/Search/{id:length(24)}")]
        public async Task<IActionResult> Search(string id, [FromForm] YouTubeVideo newVideo)
        {

            var playlist = await _PlaylistService.GetOneAsync(newVideo.PlaylistId);
            
            newVideo.id = null;
            
            await _VideoService.CreateAsync(newVideo);
            playlist.Videos.Add(newVideo.id);
            await _PlaylistService.UpdateAsync(id,playlist);
            
            ViewBag.Playlist = playlist;

            return Redirect("/Search/Search/"+id);
            //return View();
            //return CreatedAtAction(nameof(Get), new { id = newVideo.id }, newVideo);
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

        private async Task<ICollection<string>> fetchVideos(string searchKeyword) {
           
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyBDzw7qBP8m7g1CYYHn7aIxY40W5NJZPGQ",
                ApplicationName = this.GetType().ToString()
            });

            var searchListRequest = youtubeService.Search.List("snippet");

            searchListRequest.Q = searchKeyword;
            searchListRequest.MaxResults = 10;

            var searchListResponse = await searchListRequest.ExecuteAsync();

            ICollection<string> videos = new List<string>();

            foreach (var searchResult in searchListResponse.Items)
            {
                switch (searchResult.Id.Kind)
                {
                    case "youtube#video":
                        videos.Add(String.Format("{0}", searchResult.Id.VideoId));
                        break;

                }
            }          
            return videos;      
        }

        private async Task<YouTubeVideo> setupVideo(string playlistID, string vidID) {
            YouTubeVideo videoDetails = new YouTubeVideo();
            using (var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyBDzw7qBP8m7g1CYYHn7aIxY40W5NJZPGQ",
            }))
                
            {
                var searchRequest = youtubeService.Videos.List("snippet");
                searchRequest.Id = vidID;

                var searchResponse = await searchRequest.ExecuteAsync();
                var youTubeVideo = searchResponse.Items.FirstOrDefault();
                if (youTubeVideo != null)
                {
                    videoDetails.id = "JA88akl29Jhayqt6chHaNems";
                    videoDetails.Video_id = youTubeVideo.Id;
                    videoDetails.VideoTitle = youTubeVideo.Snippet.Title;
                    videoDetails.Thumbnail = youTubeVideo.Snippet.Thumbnails.High.Url;
                    videoDetails.PlaylistId = playlistID;
                    
                }
            }
            return videoDetails;
        }
        
        public async Task<List<YouTubeVideo>> Get() => await _VideoService.GetAsync();
    }

}

