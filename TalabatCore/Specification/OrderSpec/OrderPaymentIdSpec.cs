using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities.Order;

namespace TalabatCore.Specification.OrderSpec
{
    public class OrderPaymentIdSpec : BaseSepcificatin<Order>
    {
        public OrderPaymentIdSpec(string PaymentIntendId) : base(x => x.PaymentIntentId == PaymentIntendId)
        {
        }
    }
}
