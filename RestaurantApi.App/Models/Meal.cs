namespace RestaurantApi.App.Models
{
    public class Meal
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<Ingredient>? Ingredients { get; set; }
    }
}
