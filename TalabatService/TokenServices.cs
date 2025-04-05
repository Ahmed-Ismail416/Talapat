using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities.Identity;
using TalabatCore.Services;

namespace TalabatService
{
    public class TokenServices : ITokenService
    {
        public TokenServices(IConfiguration configuration)
        {

            _Configuration = configuration;
        }


        public IConfiguration _Configuration { get; }

        public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> _UserManger)
        {
            //Payload
            //Private Claims
            var AuthClaims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, user.DisplayName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            // Adding Roles if Existed 
            var Roles = await _UserManger.GetRolesAsync(user);
            foreach (var role in Roles)
            {
                AuthClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            // AuthKey
            var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Configuration["Jwt:Key"]));

            // Token
            var Token = new JwtSecurityToken
            (
                issuer: _Configuration["Jwt:Issuer"],
                audience: _Configuration["Jwt:Audience"],
                expires: DateTime.Now.AddDays(7),
                claims: AuthClaims,
                signingCredentials: new SigningCredentials(AuthKey, SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}
