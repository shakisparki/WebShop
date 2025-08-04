using WebShop.Web.Models;

namespace WebShop.Web
{
    public class BasketState
    {
        private readonly BasketService basketService;
        private readonly CatalogApiClient catalogApiClient;

        public BasketState(BasketService basketService, CatalogApiClient catalogApiClient)
        {
            this.basketService = basketService;
            this.catalogApiClient = catalogApiClient;
        }

        private List<BasketItem> Items { get; set; } = [];
        private List<CatalogItem> CatalogItems { get; set; } = [];
        private decimal TotalPrice { get; set; } = 0.0m;

        public async Task UpdateItemAsync(CatalogItem item, int quantity)
        {
            var existingItem = Items.FirstOrDefault(i => i.ProductId == item.Id);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                if (existingItem.Quantity <= 0)
                {
                    // If the quantity is zero or less, remove the item
                    await RemoveItemAsync(existingItem.ProductId);
                }
            }
            else
            {
                if (quantity <= 0)
                {
                    // If the quantity is zero or less, do not add the item
                    return;
                }
                Items.Add(new BasketItem { ProductId = item.Id, Quantity = quantity });
            }
            TotalPrice += item.Price * quantity;

            // Update the basket in the service
            await basketService.UpdateBasketAsync(Items);
        }

        public async Task  RemoveItemAsync(int productId)
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

        public async Task LoadBasketAsync()
        {
            Items = await basketService.GetBasketAsync();
            CatalogItems = await catalogApiClient.GetItemsByIdsAsync(Items.Select(x=>x.ProductId).ToList());
            TotalPrice = Items.Sum(item => CatalogItems.FirstOrDefault(c => c.Id == item.ProductId)?.Price * item.Quantity) ?? 0.0m;
        }

        //get number of item of itemid in cart
        public int GetItemQuantity(int itemId)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == itemId);
            return item?.Quantity ?? 0;
        }

        public List<CartItem> GetCartItems()
        {
            return Items.Select(x =>
            {
                var c = CatalogItems.First(c => c.Id == x.ProductId);
                return new CartItem
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Quantity = x.Quantity,
                    Price = c.Price,
                    PictureFileName = c.PictureFileName,
                    AvailableStock = c.AvailableStock
                };
            }).ToList();
        }

        public List<BasketItem> GetBasketItems()
        {
            return Items;
        }

        public decimal GetTotalPrice()
        {
            return (decimal)TotalPrice;
        }
    }
}
