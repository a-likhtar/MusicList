using MusicList.DataAccess.DbEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicList.Application.Services.Contracts
{
    public interface ITracksService
    {
        Task<IEnumerable<Track>> GetTracksAsync();
    }
}
