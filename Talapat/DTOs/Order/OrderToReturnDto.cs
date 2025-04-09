using System.Runtime.CompilerServices;
using TalabatCore.Entities.Order;

namespace Talapat.DTOs.Order
{
    public class OrderToReturnDto
    {
         
        public int Id { get; set; }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } 
        public OrderStatus Status { get; set; }
        public Address ShippingAddress { get; set; }

        public string DeliveryMethod { get; set; } 
        public decimal DeliveryMethodCost { get; set; }


        //Navigational Property for Many to One relationship
        public IReadOnlyList<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public string PaymentIntentId { get; set; }


    }
}
