using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities.Identity;

namespace TalabatRepository.Identity
{
    public class AppIdentityDbContext: IdentityDbContext<AppUser, AppRole,string> 
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {
        }
       

    }
}
