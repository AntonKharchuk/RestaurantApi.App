
using Microsoft.EntityFrameworkCore;

using RestaurantApi.Dal.Models;
using RestaurantApi.Dal.Repositories;

namespace RestaurantApi.Dal.Tests.Repositories
{
    public class MealRepositoryTests : IDisposable
    {
        private readonly DbContextOptions<RestaurantDbContext> _options;
        private readonly RestaurantDbContext _context;

        public MealRepositoryTests()
        {
            // Use an in-memory database for testing
            _options = new DbContextOptionsBuilder<RestaurantDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDatabase_{System.Guid.NewGuid()}")
                .Options;

            _context = new RestaurantDbContext(_options);
        }
        [Fact]
        public async Task GetAll_ReturnsCorrectNumOfMeals()
        {
            // Arrange
            var repository = new MealRepository(_context);
            await repository.Add(new Meal
            {
                Id = 1,
                Name = "Meal1",
                Description = "Description1",
                Ingredients = new List<Ingredient> { new Ingredient()
            {
                Id = 1,
                Name = "Ingredient1"
            }},
            });
            await repository.Add(new Meal
            {
                Id = 2,
                Name = "Meal2",
                Description = "Description2",
                Ingredients = new List<Ingredient> {  new Ingredient()
            {
                Id = 1,
                Name = "Ingredient1"
            } },
            });
            await repository.Save();

            // Act
            var result = await repository.GetAll();

            // Assert
            Assert.Equal(2, result.Count());

        }
        [Fact]
        public async Task GetAll_ReturnsCorrectIngredientsMeals()
        {
            // Arrange
            var repository = new MealRepository(_context);
            await repository.Add(new Meal
            {
                Id = 1,
                Name = "Meal1",
                Description = "Description1",
                Ingredients = new List<Ingredient> { new Ingredient()
            {
                Id = 1,
                Name = "Ingredient1"
            }},
            });
            await repository.Add(new Meal
            {
                Id = 2,
                Name = "Meal2",
                Description = "Description2",
                Ingredients = new List<Ingredient> {  new Ingredient()
            {
                Id = 2,
                Name = "Ingredient2"
            } },
            });
            await repository.Save();

            // Act
            var result = await repository.GetAll();

            // Assert
            Assert.Contains(result, meal => meal.Id == 1 && meal.Name == "Meal1");
            Assert.Contains(result, meal => meal.Id == 2 && meal.Name == "Meal2");
        }
        [Fact]
        public async Task GetAll_ReturnsEmptyListOfMealsForNoMealsAdded()
        {
            // Arrange
            var repository = new MealRepository(_context);
            await repository.Save();

            // Act
            var result = await repository.GetAll();

            // Assert
            Assert.Empty(result);
        }
        [Fact]
        public async Task GetById_ReturnsEntityWhenExists()
        {
            // Arrange
            var repository = new MealRepository(_context);
            var expectedEntity = new Meal { Id = 1, Name = "Meal1" };
            await repository.Add(expectedEntity);
            await repository.Save();

            // Act
            var result = await repository.GetById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedEntity.Id, result.Id);
            Assert.Equal(expectedEntity.Name, result.Name);
        }

        [Fact]
        public async Task GetById_ThrowsExceptionWhenEntityDoesNotExist()
        {
            // Arrange
            var repository = new MealRepository(_context);

            // Act & Assert
            await AssertThrowsArgumentNullExceptionAsync(repository.GetById(1));
        }

        [Fact]
        public async Task GetById_ThrowsExceptionWhenEntityIsNull()
        {
            // Arrange
            var repository = new MealRepository(_context);
            await repository.Add(new Meal { Id = 1, Name = "Meal1" });
            await repository.Save();

            // Act & Assert
            await AssertThrowsArgumentNullExceptionAsync(repository.GetById(2));
        }

        [Fact]
        public async Task Add_AddsEntityToDatabase()
        {
            // Arrange
            var repository = new MealRepository(_context);
            var entity = new Meal { Id = 1, Name = "Meal1" };

            // Act
            await repository.Add(entity);
            await repository.Save();

            // Assert
            var result = _context.Set<Meal>().FirstOrDefault(e => e.Id == 1);
            Assert.NotNull(result);
            Assert.Equal(entity.Name, result.Name);
        }

        [Fact]
        public async Task Add_ThrowsArgumentNullExceptionWhenEntityIsNull()
        {
            // Arrange
            var repository = new MealRepository(_context);
            Meal entity = null!;

            // Act & Assert
            await AssertThrowsArgumentNullExceptionAsync(repository.Add(entity));
        }

        [Fact]
        public async Task Update_UpdatesEntityToDatabase()
        {
            // Arrange
            var repository = new MealRepository(_context);
            var entity = new Meal { Id = 1, Name = "Meal1" };
            await repository.Add(entity);
            await repository.Save();

            var updatedEntity = new Meal { Id = 1, Name = "NewIngredient1" };

            // Act

            await repository.Update(updatedEntity, 1);
            await repository.Save();

            // Assert

            var result = _context.Set<Meal>().FirstOrDefault(e => e.Id == 1);
            Assert.NotNull(result);
            Assert.Equal(updatedEntity.Name, result.Name);
        }

        [Fact]
        public async Task Save_PersistsChangesToDatabase()
        {
            // Arrange
            var repository = new MealRepository(_context);
            var entity = new Meal { Id = 1, Name = "Meal1" };
            await repository.Add(entity);

            // Act
            await repository.Save();

            // Assert
            var result = _context.Set<Meal>().FirstOrDefault(e => e.Id == 1);
            Assert.NotNull(result);
            Assert.Equal(entity.Name, result.Name);
        }

        [Fact]
        public async Task GetInIdRange_ReturnsItemsInSpecifiedRange()
        {
            // Arrange
            var repository = new MealRepository(_context);
            await repository.Add(new Meal { Id = 1, Name = "Meal1" });
            await repository.Add(new Meal { Id = 2, Name = "Meal2" });
            await repository.Add(new Meal { Id = 3, Name = "Meal3" });
            await repository.Save();

            // Act
            var result = await repository.GetInIdRange(2, 3);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, meal => meal.Id == 2 && meal.Name == "Meal2");
            Assert.Contains(result, meal => meal.Id == 3 && meal.Name == "Meal3");
        }

        [Fact]
        public async Task GetInIdRange_ReturnsEmptyListIfNoItemsInSpecifiedRange()
        {
            // Arrange
            var repository = new MealRepository(_context);
            await repository.Add(new Meal { Id = 1, Name = "Meal1" });
            await repository.Add(new Meal { Id = 2, Name = "Meal2" });
            await repository.Save();

            // Act
            var result = await repository.GetInIdRange(3, 4);

            // Assert
            Assert.Empty(result);
        }

        public void Dispose()
        {
            // Clean up the in-memory database after each test
            _context.Dispose();
        }

        private async Task AssertThrowsArgumentNullExceptionAsync(Task task)
        {
            await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await task);
        }
    }
}
