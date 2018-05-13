using MusicList.Application.Services.Contracts;
using MusicList.DataAccess.DbEntities;
using MusicList.DataAccess.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicList.Application.Services
{
    public class TracksService : ITracksService
    {
        private readonly ITracksRepository _tracksRepository;

        public TracksService(ITracksRepository tracksRepository)
        {
            _tracksRepository = tracksRepository;
        }

        public async Task<IEnumerable<Track>> GetTracksAsync()
        {
            return await _tracksRepository.GetTracksAsync();
        }
    }
}
