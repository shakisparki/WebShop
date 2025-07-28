using Grpc.Core;
using WebShop.BasketAPI.Repositories;

namespace WebShop.BasketAPI.Services;

public class BasketService(ILogger<BasketService> logger, IBasketRepository basketRepository) : Basket.BasketBase
{
    public override async Task<BasketResponse> GetBasket(GetBasketRequest request, ServerCallContext context)
    {
        return new BasketResponse();
    }

    public override async Task<BasketResponse> AddToBasket(AddToBasketRequest request, ServerCallContext context)
    {
        logger.LogInformation("Adding item to basket for user {UserId}", request.UserId);
        await basketRepository.SetBasketAsync(request.UserId, request);
        return new BasketResponse
        {
            Success = true,
            Message = "Item added to basket successfully."
        };
        //return base.AddToBasket(request, context);
    }

    public override async Task<BasketResponse> RemoveFromBasket(RemoveFromBasketRequest request, ServerCallContext context)
    {
        logger.LogInformation("Removing item from basket for user {UserId}", request.UserId);
        await basketRepository.DeleteBasketAsync(request.UserId);
        return new BasketResponse
        {
            Success = true,
            Message = "Item removed from basket successfully."
        };
    }
}
