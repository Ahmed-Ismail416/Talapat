using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities.Basket;
using TalabatCore.Entities.Order;
using TalabatCore.Repositories;
using TalabatCore.Services.IPayment;
using TalabatCore.Services.IUnitOfWork;
using TalabatCore.Specification.OrderSpec;
using Product = TalabatCore.Entities.Product;
namespace TalabatService.Payment
{
    public class PaymentService : IPaymentService
    {
        public PaymentService(IConfiguration configure, IBasketRepositories BasketRepo, IUnitOfWork unitofwork)
        {
            _Configure = configure;
            this._BasketRepo = BasketRepo;
            _Unitofwork = unitofwork;
        }

        public IConfiguration _Configure { get; }
        public IBasketRepositories _BasketRepo { get; }
        public IUnitOfWork _Unitofwork { get; }

        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string BasketId)
        {
            
            // TODO: Prepare Stripe
            StripeConfiguration.ApiKey = _Configure["StripeSettings:SecretKey"];

            // TODO: Get Basket
            var Basket = await _BasketRepo.GetBasketAsync(BasketId);
            // TODO : ShippingPrice 
            var ShippingPrice = 0M;
            if(Basket?.DelvieryMethodId is not null)
            {
                var DelvieryMethod = await _Unitofwork.Repostiory<DeliveryMethod>().GetbyIdAsync((int)Basket.DelvieryMethodId);
                ShippingPrice = DelvieryMethod.Cost;
            }

            // TODO: Get Price Of Items From DB
            if(Basket.Items.Count > 0)
            {
                foreach (var item in Basket.Items)
                {
                    var Product = await _Unitofwork.Repostiory<Product>().GetbyIdAsync(item.Id);
                    if(Product.Price != item.Price)
                    {
                        item.Price = Product.Price;
                    }
                }
            }

            // TODO: Get SubTotal
            var SubTotal = Basket.Items.Sum(i => i.Price * i.Quantity);

            // TODO: Create PaymentIntent
            var Service = new PaymentIntentService();
            PaymentIntent PaymentIntent;
            if (string.IsNullOrEmpty(Basket.PaymentIntentId))
            {
                // Create Pament
                var Options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)SubTotal * 100+ (long)ShippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card"},
                };
                PaymentIntent = await  Service.CreateAsync(Options);
                Basket.PaymentIntentId = PaymentIntent.Id;
                Basket.ClientSecret = PaymentIntent.ClientSecret;
            }
            else
            {
                //Update Payment
                var Options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)SubTotal * 100 + (long)ShippingPrice * 100,
                };
                PaymentIntent = await Service.UpdateAsync(Basket.PaymentIntentId, Options);
                Basket.PaymentIntentId = PaymentIntent.Id;
                Basket.ClientSecret = PaymentIntent.ClientSecret;
            }
            _BasketRepo.UpdateBasketAsync(Basket);
            return Basket;

        }

        public async Task<Order> UpdatePaymentIntentToFailedOrSucced(string PaymentIntendId, bool flag)
        {
            var spec = new OrderPaymentIdSpec(PaymentIntendId);
            var order = await _Unitofwork.Repostiory<Order>().GetEntityWithSpecAsync(spec);
            if(flag)
            {
                order.Status = OrderStatus.PaymentReceived;
            }
            else
            {
                order.Status = OrderStatus.PaimentFailed;
            }
            _Unitofwork.Repostiory<Order>().Update(order);
            await _Unitofwork.CompleteAsync();
            return order;
        }
    }
}
