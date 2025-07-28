
using WebShop.BasketAPI.Models;

namespace WebShop.BasketAPI.Repositories
{
    public interface IBasketRepository
    {
        Task DeleteBasketAsync(string userId);
        Task<BasketResource?> GetBasketAsync(string userId);
        Task SetBasketAsync(BasketResource basket);
    }
}