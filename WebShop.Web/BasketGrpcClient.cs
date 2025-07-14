using WebShop.BasketAPI;
using GrpcClient = WebShop.BasketAPI.Basket.BasketClient;

namespace WebShop.Web
{
    public class BasketGrpcClient(GrpcClient Client)
    {
        private readonly GrpcClient _client = Client;
        public async Task<BasketResponse> AddToBasketAsync(BasketRequest request)
        {
            return await _client.AddToBasketAsync(request);
        }
        public async Task<BasketResponse> RemoveFromBasketAsync(BasketRequest request)
        {
            return await _client.RemoveFromBasketAsync(request);
        }
        //public async Task<BasketResponse> GetBasketAsync(string userId)
        //{
        //    var request = new BasketRequest { UserId = userId };
        //    return await _client.GetBasketAsync(request);
        //}
    }
}
