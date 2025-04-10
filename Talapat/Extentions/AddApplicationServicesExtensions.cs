using TalabatCore.Repositories;
using TalabatCore.Services;
using TalabatCore.Services.IPayment;
using TalabatCore.Services.IUnitOfWork;
using TalabatRepository;
using TalabatRepository.UnitOfWork;
using TalabatService;
using TalabatService.Payment;
using Talapat.Helpers;

namespace Talapat.Extentions
{
    public static class AddApplicationServicesExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericReposatory<>));
            Services.AddScoped<IBasketRepositories, BasketRepositories>();
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddScoped<IOrderService, OrderService>();
            Services.AddScoped<IPaymentService, PaymentService>();
            Services.AddAutoMapper(typeof(ProductProfile));
            
            return Services;
        }

    }
}
