
using Microsoft.EntityFrameworkCore;

using RestaurantApi.Dal.Models;

namespace RestaurantApi.Dal
{
    public class RestaurantDbContext : DbContext
    {
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Portion> Portions { get; set; }
        public DbSet<PriceListItem> PriceList { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options)
        : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PriceListItem>().ToTable("PriceList");

            modelBuilder.Entity<Meal>()
              .HasMany(m => m.Ingredients)
              .WithMany(i => i.Meals)
              .UsingEntity(j => j.ToTable("MealIngredients"));
        }
    }
}
