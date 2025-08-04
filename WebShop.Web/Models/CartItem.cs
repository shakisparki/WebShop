namespace WebShop.Web.Models
{

    public class CartItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string PictureFileName { get; set; }
        public int AvailableStock { get; set; }
    }
}
