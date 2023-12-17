
using Microsoft.EntityFrameworkCore;

using RestaurantApi.Dal.Models;
using RestaurantApi.Dal.Repositories;
using RestaurantApi.Dal.Services;

namespace RestaurantApi.Dal.Tests.Services;

public class MealCRUDServiceTests
{
    private MealRepository _repository;
    private MealCRUDService _testService;

    public MealCRUDServiceTests()
    {
        // Use an in-memory database for testing
        var options = new DbContextOptionsBuilder<RestaurantDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDatabase_{System.Guid.NewGuid()}")
            .Options;

        var context = new RestaurantDbContext(options);

        _repository = new MealRepository(context);

        _testService = new MealCRUDService(_repository);
    }

    [Fact]
    public async Task GetItemsInRangeAsync_ShouldReturnItemsInRangeWithInclude()
    {
        // Arrange: Add test data
        var meal1 = new Meal { Id = 1, Name = "Meal1" };
        var meal2 = new Meal { Id = 2, Name = "Meal2" };
        var meal3 = new Meal { Id = 3, Name = "Meal3" };
        await _repository.AddAsync(meal1);
        await _repository.AddAsync(meal2);
        await _repository.AddAsync(meal3);
        await _repository.SaveAsync();

        // Act
        var result = await _testService.GetItemsInRangeAsync(1, 2, "Ingredients");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());

        // Check that the included property (Ingredients) is loaded
        var firstMeal = result.First();
        Assert.NotNull(firstMeal.Ingredients);
    }

    [Fact]
    public async Task GetItemsInRangeAsync_ShouldReturnItemsInRangeWithoutInclude()
    {
        // Arrange: Add test data
        var meal1 = new Meal { Id = 1, Name = "Meal1" };
        var meal2 = new Meal { Id = 2, Name = "Meal2" };
        var meal3 = new Meal { Id = 3, Name = "Meal3" };
        await _repository.AddAsync(meal1);
        await _repository.AddAsync(meal2);
        await _repository.AddAsync(meal3);
        await _repository.SaveAsync();

        // Act
        var result = await _testService.GetItemsInRangeAsync(1, 2);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());

        // Check that the included property (Ingredients) is not loaded
        var firstMeal = result.First();
        Assert.NotNull(firstMeal.Ingredients);
        Assert.Empty(firstMeal.Ingredients);
    }

    [Fact]
    public async Task GetItemsInRangeAsync_ShouldReturnEmptyListForInvalidRange()
    {
        // Act
        var result = await _testService.GetItemsInRangeAsync(5, 10);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

}
