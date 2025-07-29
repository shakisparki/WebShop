using WebShop.BasketAPI;
using GrpcClient = WebShop.BasketAPI.Basket.BasketClient;
using GrpcBasketItem = WebShop.BasketAPI.BasketItem;
using BasketItem = WebShop.Web.Models.BasketItem;

namespace WebShop.Web
{
    public class BasketService(GrpcClient Client)
    {
        private readonly GrpcClient _client = Client;

        public async Task<List<BasketItem>> GetBasketAsync()
        {
            var basket = await _client.GetBasketAsync(new GetBasketRequest());
            var items = basket.Items.Select(item => new BasketItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity
            }).ToList();
            return [.. items];
        }

        public async Task UpdateBasketAsync(List<BasketItem> basket)
        {
            var request = new UpdateBasketRequest();
            foreach (var item in basket)
            {
                request.Items.Add(new GrpcBasketItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                });
            }
            await _client.UpdateBasketAsync(request);
        }
        public async Task RemoveBasketAsync()
            => await _client.RemoveBasketAsync(new RemoveBasketRequest());
    }
}
