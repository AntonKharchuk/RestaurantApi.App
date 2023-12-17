namespace RestaurantApi.App.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string? CustomerName { get; set; }
        public DateTime Date { get; set; }
    }
}
