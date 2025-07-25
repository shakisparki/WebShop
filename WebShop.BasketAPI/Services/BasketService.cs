using Grpc.Core;
using WebShop.BasketAPI.Repositories;

namespace WebShop.BasketAPI.Services;

public class BasketService : Basket.BasketBase
{
    private readonly ILogger<BasketService> _logger;
    private readonly IBasketRepository _basketRepository;

    public BasketService(ILogger<BasketService> logger, IBasketRepository basketRepository)
    {
        _logger = logger;
        _basketRepository = basketRepository;
    }

    public override async Task<BasketResponse> GetBasket(GetBasketRequest request, ServerCallContext context)
    {
        return new BasketResponse();
    }

    public override async Task<BasketResponse> AddToBasket(AddToBasketRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Adding item to basket for user {UserId}", request.UserId);
        await _basketRepository.SetBasketAsync(request.UserId, request);
        return new BasketResponse
        {
            Success = true,
            Message = "Item added to basket successfully."
        };
        //return base.AddToBasket(request, context);
    }

    public override async Task<BasketResponse> RemoveFromBasket(RemoveFromBasketRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Removing item from basket for user {UserId}", request.UserId);
        await _basketRepository.DeleteBasketAsync(request.UserId);
        return new BasketResponse
        {
            Success = true,
            Message = "Item removed from basket successfully."
        };
    }
}
