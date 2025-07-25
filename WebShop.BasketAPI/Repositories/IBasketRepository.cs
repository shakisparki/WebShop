
namespace WebShop.BasketAPI.Repositories
{
    public interface IBasketRepository
    {
        Task DeleteBasketAsync(int userId);
        Task<AddToBasketRequest?> GetBasketAsync(int userId);
        Task SetBasketAsync(int userId, AddToBasketRequest basket);
    }
}