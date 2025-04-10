using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities.Basket;
using TalabatCore.Entities.Order;

namespace TalabatCore.Services.IPayment
{
    public interface IPaymentService
    {
        public Task<CustomerBasket> CreateOrUpdatePaymentIntent(string BasketId);
        public Task<Order> UpdatePaymentIntentToFailedOrSucced(string PaymentIntendId, bool flag);
    }
}
