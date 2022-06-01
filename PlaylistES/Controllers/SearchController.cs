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

        private readonly UserService _UserService;
        private readonly YtVideoService _VideoService;
        private readonly PlaylistsService _PlaylistService;


        public SearchController(UserService userService,YtVideoService videoService, PlaylistsService playlistService)
        {
            _UserService = userService;
            _VideoService = videoService;
            _PlaylistService = playlistService;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Search(string id)
        {
            ICollection<YouTubeVideo> allVideos = new List<YouTubeVideo>();
            var playlist = await _PlaylistService.GetOneAsync(id);
            string urlQuery = HttpContext.Request.QueryString.ToString();
            var user = await _UserService.GetOneIDAsync(playlist.Creator_id);

            if (urlQuery.Length > 3)
            {
                string searchKeyword = urlQuery.Substring(3);

                foreach (var video in await fetchVideos(searchKeyword))
                {
                    allVideos.Add(await setupVideo(id, video));
                }

            }            

            ViewBag.Playlist = playlist;
            ViewBag.user = user;

            return View(allVideos);
        }




        [HttpPost("{id}")]
        public async Task<IActionResult> Search(string id, [FromForm] YouTubeVideo newVideo)
        {

            var playlist = await _PlaylistService.GetOneAsync(newVideo.PlaylistId);
            
            newVideo.id = null;
            
            await _VideoService.CreateAsync(newVideo);
            playlist.Videos.Add(newVideo.id);
            await _PlaylistService.UpdateAsync(id,playlist);
            
            ViewBag.Playlist = playlist;

            return Redirect("/Search/Search/"+id);
        }


        private async Task<ICollection<string>> fetchVideos(string searchKeyword) {
           
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyBDzw7qBP8m7g1CYYHn7aIxY40W5NJZPGQ",
                ApplicationName = this.GetType().ToString()
            });

            var searchListRequest = youtubeService.Search.List("snippet");

            searchListRequest.Q = searchKeyword;
            searchListRequest.MaxResults = 20;

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
    }

}

