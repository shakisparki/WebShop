
using WebShop.BasketAPI.Models;

namespace WebShop.BasketAPI.Repositories
{
    public interface IBasketRepository
    {
        Task DeleteBasketAsync(int userId);
        Task<BasketResource?> GetBasketAsync(int userId);
        Task SetBasketAsync(BasketResource basket);
    }
}