using Grpc.Core;
using WebShop.BasketAPI.Models;
using WebShop.BasketAPI.Repositories;

namespace WebShop.BasketAPI.Services;

public class BasketService(ILogger<BasketService> logger, IBasketRepository basketRepository) : Basket.BasketBase
{
    public override async Task<BasketResponse> GetBasket(GetBasketRequest request, ServerCallContext context)
    {
        var userId = context.GetHttpContext().User.FindFirst("sub")?.Value ?? "testuser";
        var basket = await basketRepository.GetBasketAsync(userId);
        var response = new BasketResponse();
        if( basket is null)
        {
            return response;
        }
        basket.Items.ForEach(item =>
        {
            response.Items.Add(new BasketItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
            });
        });
        return response;
    }

    public override async Task<BasketResponse> UpdateBasket(UpdateBasketRequest request, ServerCallContext context)
    {
        var userId = context.GetHttpContext().User.FindFirst("sub")?.Value ?? "testuser";
        logger.LogInformation("Adding item to basket for user {UserId}", userId);

        var basketResource = new BasketResource
        {
            UserId = userId,
            Items = request.Items.Select(item => new BasketItemResource
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity
            }).ToList()
        };
        await basketRepository.SetBasketAsync(basketResource);
        return new BasketResponse
        {
            Items = { request.Items }
        };  
    }

    public override async Task<RemoveBasketResponse> RemoveBasket(RemoveBasketRequest request, ServerCallContext context)
    {
        var userId = context.GetHttpContext().User.FindFirst("sub")?.Value ?? "testuser";
        logger.LogInformation("Removing item from basket for user {UserId}", userId);
        await basketRepository.DeleteBasketAsync(userId);
        return new RemoveBasketResponse{};
    }
}
