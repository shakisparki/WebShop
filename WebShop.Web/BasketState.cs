using WebShop.Web.Models;

namespace WebShop.Web
{
    public class BasketState(BasketService basketService, CatalogApiClient catalogApiClient)
    {
        public List<BasketItem> Items { get; set; } = [];
        public List<CatalogItem> CatalogItems { get; set; } = [];
        public decimal TotalPrice { get; set; } = 0.0m;

        public async Task AddItem(CatalogItem item, int quantity)
        {
            var existingItem = Items.FirstOrDefault(i => i.ProductId == item.Id);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                Items.Add(new BasketItem { ProductId = item.Id, Quantity = quantity });
            }
            TotalPrice += item.Price * quantity;

            // Update the basket in the service
            await basketService.UpdateBasketAsync(Items);
        }

        public async Task  RemoveItem(int productId)
        {
            var itemToRemove = Items.FirstOrDefault(i => i.ProductId == productId);
            var catalogitem = CatalogItems.FirstOrDefault(i => i.Id == productId);
            if (itemToRemove != null && catalogitem != null)
            {
                TotalPrice -= itemToRemove.Quantity * catalogitem.Price;
                Items.Remove(itemToRemove);

                // Update the basket in the service
                await basketService.UpdateBasketAsync(Items);
            }
        }

        public async Task LoadBasket()
        {
            Items = await basketService.GetBasketAsync();
            CatalogItems = await catalogApiClient.GetItemsByIdsAsync(Items.Select(x=>x.ProductId).ToList());
            TotalPrice = Items.Sum(item => CatalogItems.FirstOrDefault(c => c.Id == item.ProductId)?.Price * item.Quantity) ?? 0.0m;
        }
    }
}
