
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Threading.Tasks;
using TalabatCore.Entities;
using TalabatCore.Repositories;
using TalabatRepository;
using TalabatRepository.Data;
using TalabatRepository.Identity;
using Talapat.Extentions;
using Talapat.Helpers;


namespace Talapat
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //use top level statement
            var builder = WebApplication.CreateBuilder(args);

            
            #region Services

            builder.Services.AddControllers();
            builder.Services.AddDbContext<StoreContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"))
                );
            builder.Services.AddSingleton<IConnectionMultiplexer>(option =>
            {
                var Connection = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(Connection);

            }
            );
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddApplicationServices();
            #endregion


            var app = builder.Build();

            #region UpdateDataBase
            
            using var Scope = app.Services.CreateScope();
            // Group of services liftime scoped
            var Services = Scope.ServiceProvider;
            var loggerfactory = Services.GetRequiredService<ILoggerFactory>();
            try
            {
                //Services itself
                var dbcontext = Services.GetRequiredService<StoreContext>();
                var identityContext = Services.GetRequiredService<AppIdentityDbContext>();
                // Ask CLR to Create Object From Dbcontext excplicity
                await dbcontext.Database.MigrateAsync();
                await identityContext.Database.MigrateAsync();
                await StoreContextSeed.SeedAsync(dbcontext);
            }
            catch(Exception ex)
            {
                var logger = loggerfactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occurred during migration");
            }
            
            #endregion

            #region Configure the HTTP request pipeline
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.AddSwagger();
            }
            //use resources
            app.UseStaticFiles();
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
#endregion 

            app.Run();

        }
    }
    
}
       