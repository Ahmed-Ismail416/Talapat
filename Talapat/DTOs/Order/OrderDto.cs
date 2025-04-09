using System.ComponentModel.DataAnnotations;

namespace Talapat.DTOs.Order
{
    public class OrderDto
    {
        [Required]
        public string BasketId { get; set; }
        [Required]
        public int DeliverMethodId { get; set; }
        [Required]
        public AddressDto ShippingAddress { get; set; }
    }
}
