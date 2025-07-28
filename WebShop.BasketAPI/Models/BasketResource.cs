namespace WebShop.BasketAPI.Models
{
    public class BasketResource
    {
        public int UserId { get; set; }
        public List<BasketItemResource> Items { get; set; } = new List<BasketItemResource>();
    }

    public class BasketItemResource
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
