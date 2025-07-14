
namespace WebShop.BasketAPI.Repositories
{
    public interface IBasketRepository
    {
        Task DeleteBasketAsync(int userId);
        Task<BasketRequest?> GetBasketAsync(int userId);
        Task SetBasketAsync(int userId, BasketRequest basket);
    }
}