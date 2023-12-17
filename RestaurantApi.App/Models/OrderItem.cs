namespace RestaurantApi.App.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public string? Coment { get; set; }
        public bool IsReady { get; set; }
        public Order? Order { get; set; }
        public int OrderId { get; set; }
        public PriceListItem? PriceListItem { get; set; }
        public int PriceListItemId { get; set; }
    }
}
