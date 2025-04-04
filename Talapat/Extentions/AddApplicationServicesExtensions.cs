using TalabatCore.Repositories;
using TalabatRepository;
using Talapat.Helpers;

namespace Talapat.Extentions
{
    public static class AddApplicationServicesExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericReposatory<>));
            Services.AddScoped<IBasketRepositories, BasketRepositories>();
            Services.AddAutoMapper(typeof(ProductProfile));
            return Services;
        }

    }
}
