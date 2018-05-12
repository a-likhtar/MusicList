using MusicList.DataAccess.DbEntities.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicList.Application.Services.Auth.Contracts
{
    public interface ITokenGenerator
    {
        Task<string> GenerateJwtToken(ApplicationUser user);
    }
}
