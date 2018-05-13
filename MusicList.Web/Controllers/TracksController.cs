using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicList.Application.Services.Contracts;

namespace MusicList.Web.Controllers
{
    [Produces("application/json")]
    [Route("tracks")]
    public class TracksController : Controller
    {
        private readonly ITracksService _tracksService;

        public TracksController(ITracksService tracksService)
        {
            _tracksService = tracksService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTracks()
        {
            return Ok(await _tracksService.GetTracksAsync());
        }
    }
}
