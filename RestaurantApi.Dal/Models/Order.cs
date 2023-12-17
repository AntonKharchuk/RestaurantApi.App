
namespace RestaurantApi.Dal.Models
{
    public class Order: BaseEntity
    {
        public string? CustomerName { get; set; }
        public DateTime Date { get; set; }
    }
}
