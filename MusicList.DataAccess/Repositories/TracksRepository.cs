using MusicList.DataAccess.DbEntities;
using MusicList.DataAccess.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicList.DataAccess.Repositories
{
    public class TracksRepository : ITracksRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TracksRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Track>> GetTracksAsync()
        {
            return _dbContext.Tracks;
        }
    }
}
