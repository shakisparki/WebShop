namespace WebShop.BasketAPI.Models
{
    public class BasketResource
    {
        public required string UserId { get; set; }
        public List<BasketItemResource> Items { get; set; } = [];
    }

    public class BasketItemResource
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
