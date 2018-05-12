using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MusicList.Application.Services.Auth.Contracts;
using MusicList.CrosscuttingConcerns;
using MusicList.DataAccess.DbEntities.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MusicList.Application.Services.Auth
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly AppSettings _cnfg;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly int _lifetime;

        public TokenGenerator(AppSettings cnfg,
                                UserManager<ApplicationUser> userManager)
        {
            _cnfg = cnfg;
            _lifetime = int.Parse(_cnfg.AuthOptions.Lifetime ?? int.MaxValue.ToString());
            _userManager = userManager;
        }

        public async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var now = DateTime.UtcNow;
            var userRoles = await _userManager.GetRolesAsync(user);

            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
            userClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));


            // create JWT-token
            var jwt = new JwtSecurityToken(
                _cnfg.AuthOptions.Issuer,
                _cnfg.AuthOptions.Audience,
                notBefore: now,
                claims: userClaims,
                expires: now.Add(TimeSpan.FromMinutes(_lifetime)),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_cnfg.AuthOptions.Key)),
                    SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
