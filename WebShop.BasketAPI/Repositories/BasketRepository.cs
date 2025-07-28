using StackExchange.Redis;
using System.Text.Json;
using WebShop.BasketAPI.Models;

namespace WebShop.BasketAPI.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        private readonly string _basketPrefix = "/basket/";

        public BasketRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            _database = connectionMultiplexer.GetDatabase();
        }

        public async Task SetBasketAsync(BasketResource basket)
        {
            var serializedBasket = JsonSerializer.Serialize(basket);
            await _database.StringSetAsync(GetBasketKey(basket.UserId), serializedBasket);
        }

        public async Task<BasketResource?> GetBasketAsync(int userId)
        {
            var basket = await _database.StringGetAsync(GetBasketKey(userId));
            return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<BasketResource>(basket);
        }

        public async Task DeleteBasketAsync(int userId)
        {
            await _database.KeyDeleteAsync(GetBasketKey(userId));
        }

        private string GetBasketKey(int userId)
        {
            return $"{_basketPrefix}{userId}";
        }
    }
}
