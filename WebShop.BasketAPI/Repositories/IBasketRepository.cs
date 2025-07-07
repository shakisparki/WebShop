
namespace WebShop.BasketAPI.Repositories
{
    public interface IBasketRepository
    {
        Task DeleteBasketAsync(string userId);
        Task<BasketRequest?> GetBasketAsync(string userId);
        Task SetBasketAsync(string userId, BasketRequest basket);
    }
}