
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

        public RestaurantDbContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=helloapp.db");
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
