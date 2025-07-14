using StackExchange.Redis;
using System.Text.Json;

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

        public async Task SetBasketAsync(int userId, BasketRequest basket)
        {
            var serializedBasket = JsonSerializer.Serialize(basket);
            await _database.StringSetAsync(GetBasketKey(userId), serializedBasket);
        }

        public async Task<BasketRequest?> GetBasketAsync(int userId)
        {
            var basket = await _database.StringGetAsync(GetBasketKey(userId));
            return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<BasketRequest>(basket);
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
