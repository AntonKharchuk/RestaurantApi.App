
namespace RestaurantApi.Dal.Models
{
    public class OrderItem: BaseEntity
    {
        public string? Coment { get; set; }
        public bool IsReady { get; set; }
        public Order? Order { get; set; }
        public int OrderId { get; set; }
        public PriceListItem? PriceListItem;
        public int PriceListItemId { get; set; }
    }
}
