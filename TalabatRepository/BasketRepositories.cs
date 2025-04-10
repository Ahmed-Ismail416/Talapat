using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using TalabatCore.Entities.Basket;
using TalabatCore.Repositories;

namespace TalabatRepository
{
    public class BasketRepositories : IBasketRepositories
    {
        public IDatabase _DataBase { get; }
        public BasketRepositories(IConnectionMultiplexer Redis)
        {
            _DataBase = Redis.GetDatabase();
        }


        public async Task<bool> DeleteBasketAsync(string BasketId)
        {
            return await _DataBase.KeyDeleteAsync(BasketId);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string BasketId)
        {
            var Basket = await _DataBase.StringGetAsync(BasketId);
            return Basket.IsNullOrEmpty? null: JsonSerializer.Deserialize<CustomerBasket>(Basket);

        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
            var JsonBasket = JsonSerializer.Serialize(basket);
            var CreatedOrUpdated = await _DataBase.StringSetAsync(basket.Id, JsonBasket, TimeSpan.FromDays(1));
            if(CreatedOrUpdated)
            {
                return await GetBasketAsync(basket.Id);
            }
            return null;
        }
    }
}
