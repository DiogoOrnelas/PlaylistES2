﻿using PlaylistES.Models;
using PlaylistES.Services;
using Microsoft.AspNetCore.Mvc;

namespace PlaylistES.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PlaylistsController : Controller
    {

        private readonly PlaylistsService _PlaylistService;
        private readonly YtVideoService _VideoService;

        public PlaylistsController(PlaylistsService playlistService, YtVideoService videoService)
        {
            _VideoService = videoService;
            _PlaylistService = playlistService;
        }

        [HttpGet]
        public async Task<IActionResult>  MyPlaylists()
         {
            var data = await _PlaylistService.GetAsync();

            return View(data);
        }


        public async Task<List<Playlists>> Get() =>
            await _PlaylistService.GetAsync();
        
        public async Task<IActionResult> CreatePlaylist()
        {
           
            return View();
        }


        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Playlists>> Get(string id)
        {
            var playlist = await _PlaylistService.GetOneAsync(id);

            if (playlist is null)
            {
                return NotFound();
            }

            return playlist;
        }


        [HttpPost]
        public async Task<IActionResult> CreatePlaylist([FromForm] Playlists newPlaylist)
        {
            Console.WriteLine("Hello there! this is playlist: "+ newPlaylist.PlaylistName );
            newPlaylist.PlaylistId = null;
            newPlaylist.Videos.Clear();
            await _PlaylistService.CreateAsync(newPlaylist);

            //return CreatedAtAction(nameof(Get), new { id = newPlaylist.PlaylistId }, newPlaylist);
            return Redirect("/Playlists/MyPlaylists");
        }

        [HttpPost("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var videos = await _VideoService.GetAsync(id);
            var playlist = await _PlaylistService.GetOneAsync(id);
            if (videos is null)
            {
                return NotFound();
            }
            foreach (var video in videos)
            {
                await _VideoService.RemoveAsync(video.id);

            }

            await _PlaylistService.RemoveAsync(id);

            return Redirect("/Playlists/MyPlaylists");
        }


        [HttpPut("{id:length(24)}")]
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

