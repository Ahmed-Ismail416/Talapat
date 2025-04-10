using System.ComponentModel.DataAnnotations;
using TalabatCore.Entities.Basket;

namespace Talapat.DTOs.Basket
{
    public class CustomerBasketDto
    {
        [Required]
        public string Id { get; set; }
        public List<BasketItems> Items { get; set; }

        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public int? DelvieryMethodId { get; set; }
    }
}
