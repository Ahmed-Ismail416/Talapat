﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalabatCore.Entities.Basket
{
    public class CustomerBasket
    {
        public CustomerBasket()
        {
            
        }
        public string Id { get; set; }
        public List<BasketItems> Items { get; set; }

        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public int? DelvieryMethodId { get; set; }
        
    }
}
