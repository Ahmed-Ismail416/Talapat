using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TalabatCore.Entities.Identity;

namespace Talapat.Extentions
{
    public static class UserManagerExtension
    {
        public static async Task<AppUser> FindUserWithAddressAsync(this UserManager<AppUser> _usermanager, ClaimsPrincipal claim)
        {
            var Email = claim.FindFirstValue(ClaimTypes.Email);
            var User = await _usermanager.Users.Include(x => x.Address).FirstOrDefaultAsync(x => x.Email == Email);
            return User;
        }
    }
}
