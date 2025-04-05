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
    }
}
