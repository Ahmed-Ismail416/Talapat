using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities.Order;

namespace TalabatCore.Specification.OrderSpec
{
    public class OrderSpecification: BaseSepcificatin<Order>
    {
        public OrderSpecification(string buyeremail) :base(O => O.BuyerEmail == buyeremail)
        {
            Includes.Add(x => x.DeliveryMethod);
            Includes.Add(x => x.Items);
            OrderByDescending(Object => Object.OrderDate);
        }
        public OrderSpecification(string buyeremail, int orderid):base(p =>
        (p.BuyerEmail == buyeremail )
        && 
        (p.Id == orderid))
        {
            Includes.Add(x => x.DeliveryMethod);
            Includes.Add(x => x.Items);
        }
    }
}
