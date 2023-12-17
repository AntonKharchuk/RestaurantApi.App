
namespace RestaurantApi.Dal.Models
{
    public class Meal: BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<Ingredient>? Ingredients { get; set; }
    }
}
