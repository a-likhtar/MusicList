using MusicList.DataAccess.DbEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicList.DataAccess.Repositories.Contracts
{
    public interface ITracksRepository
    {
        Task<IEnumerable<Track>> GetTracksAsync();
    }
}
