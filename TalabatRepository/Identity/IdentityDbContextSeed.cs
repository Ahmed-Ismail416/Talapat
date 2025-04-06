using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities.Identity;

namespace TalabatRepository.Identity
{
    public static  class IdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> usermanager)
        {
            if(!usermanager.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName = "Test User",
                    Email = "AhmedIsmial416@gmai.com",
                    UserName = "AhmedIsmial416",
                    PhoneNumber = "01012345678",
                };
                 usermanager.CreateAsync(user, "Pa$$w0rd");   
            }
        }
        public static async Task SeedRoleAsync(RoleManager<AppRole> rolemanager)
        {
           if(!rolemanager.Roles.Any())
           {
                var Role = new AppRole()
                {
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                };
                
                var Role2 = new AppRole()
                {
                    Name = "User",
                    NormalizedName = "User".ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                };
                await rolemanager.CreateAsync(Role);
                await rolemanager.CreateAsync(Role2);
           }

        }
    }
}
