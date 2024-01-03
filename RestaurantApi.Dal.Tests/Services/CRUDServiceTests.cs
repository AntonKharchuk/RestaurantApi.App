
using Microsoft.EntityFrameworkCore;

using RestaurantApi.Dal.Exeptions;
using RestaurantApi.Dal.Models;
using RestaurantApi.Dal.Repositories;
using RestaurantApi.Business.Services;

namespace RestaurantApi.Dal.Tests.Services;

public class CRUDServiceTests
{
    private Repository<Ingredient> _repository;
    private CRUDService<Ingredient> _testService;

    public CRUDServiceTests()
    {
        // Use an in-memory database for testing
        var options = new DbContextOptionsBuilder<RestaurantDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDatabase_{System.Guid.NewGuid()}")
            .Options;

        var context = new RestaurantDbContext(options);

        _repository = new Repository<Ingredient>(context);

        _testService = new CRUDService<Ingredient>(_repository);
    }


    [Fact]
    public async Task GetAllItemsAsync_ReturnAllItemsWithInclude()
    {
        // Arrange: Add test data
        var ingredient1 = new Ingredient { Name = "Ingredient1" };
        var ingredient2 = new Ingredient { Name = "Ingredient2" };
        await _repository.AddAsync(ingredient1);
        await _repository.AddAsync(ingredient2);
        await _repository.SaveAsync();

        // Act
        var result = await _testService.GetAllItemsAsync("Meals");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());

        // Check that the included property (Meals) is loaded
        var firstIngredient = result.First();
        Assert.NotNull(firstIngredient.Meals);
    }

    [Fact]
    public async Task GetAllItemsAsync_ReturnAllItemsWithoutInclude()
    {
        // Arrange: Add test data
        var ingredient1 = new Ingredient { Name = "Ingredient1" };
        var ingredient2 = new Ingredient { Name = "Ingredient2" };
        await _repository.AddAsync(ingredient1);
        await _repository.AddAsync(ingredient2);
        await _repository.SaveAsync();

        // Act
        var result = await _testService.GetAllItemsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());

        // Check that the included property (Meals) is not loaded
        var firstIngredient = result.First();
        Assert.Null(firstIngredient.Meals);
    }

    [Fact]
    public async Task GetItemByIdAsync_ReturnItemWithInclude()
    {
        // Arrange: Add test data
        var ingredient = new Ingredient { Id = 1, Name = "Ingredient1" };
        await _repository.AddAsync(ingredient);
        await _repository.SaveAsync();

        // Act
        var result = await _testService.GetItemByIdAsync(1, "Meals");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);

        // Check that the included property (Meals) is loaded
        Assert.NotNull(result.Meals);
    }

    [Fact]
    public async Task GetItemByIdAsync_ReturnItemWithoutInclude()
    {
        // Arrange: Add test data
        var ingredient = new Ingredient { Id = 1, Name = "Ingredient1" };
        await _repository.AddAsync(ingredient);
        await _repository.SaveAsync();

        // Act
        var result = await _testService.GetItemByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);

        // Check that the included property (Meals) is not loaded
        Assert.Null(result.Meals);
    }

    [Fact]
    public async Task GetItemByIdAsync_ReturnNullForNonExistingItem()
    {
        // Act
        var result = await _testService.GetItemByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateItemAsync_ShouldCreateNewItem()
    {
        // Arrange
        var newIngredient = new Ingredient { Name = "NewIngredient" };

        // Act
        await _testService.CreateItemAsync(newIngredient);

        // Assert
        // Retrieve the created item from the repository and check its properties
        var createdItem = await _repository.GetByIdAsync(newIngredient.Id);
        Assert.NotNull(createdItem);
        Assert.Equal(newIngredient.Name, createdItem.Name);
        // Add more assertions based on the properties of your entity
    }

    [Fact]
    public async Task UpdateItemAsync_ShouldUpdateExistingItem()
    {
        // Arrange: Add test data
        var originalIngredient = new Ingredient { Id = 1, Name = "OriginalIngredient" };
        await _repository.AddAsync(originalIngredient);
        await _repository.SaveAsync();

        var updatedIngredient = new Ingredient { Id = 1, Name = "UpdatedIngredient" };

        // Act
        await _testService.UpdateItemAsync(updatedIngredient, updatedIngredient.Id);

        // Assert
        var updatedItem = await _repository.GetByIdAsync(updatedIngredient.Id);
        Assert.NotNull(updatedItem);
        Assert.Equal(updatedIngredient.Name, updatedItem.Name);
    }
    [Fact]
    public async Task DeleteItemAsync_ShouldDeleteExistingItem()
    {
        // Arrange: Add test data
        var ingredientToDelete = new Ingredient { Id = 1, Name = "IngredientToDelete" };
        await _repository.AddAsync(ingredientToDelete);
        await _repository.SaveAsync();

        // Act
        await _testService.DeleteItemAsync(ingredientToDelete.Id);

        // Assert
        // Retrieve the deleted item from the repository and ensure it is null
        var deletedItem = await _repository.GetByIdAsync(ingredientToDelete.Id);
        Assert.Null(deletedItem);
    }

    [Fact]
    public async Task DeleteItemAsync_ShouldThrowExceptionForNonExistingItem()
    {
        // Act and Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _testService.DeleteItemAsync(999));
    }




}
