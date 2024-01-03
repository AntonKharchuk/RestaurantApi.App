
using Microsoft.EntityFrameworkCore;

using RestaurantApi.Dal.Exeptions;
using RestaurantApi.Dal.Models;
using RestaurantApi.Dal.Repositories;

namespace RestaurantApi.Dal.Tests.Repositories;

public class MealRepositoryTests : IDisposable
{
    private readonly DbContextOptions<RestaurantDbContext> _options;
    private readonly RestaurantDbContext _context;

    public MealRepositoryTests()
    {
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
        await repository.AddAsync(new Meal
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
        await repository.AddAsync(new Meal
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
        await repository.SaveAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());

    }
    [Fact]
    public async Task GetAll_ReturnsCorrectIngredientsMeals()
    {
        // Arrange
        var repository = new MealRepository(_context);
        await repository.AddAsync(new Meal
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
        await repository.AddAsync(new Meal
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
        await repository.SaveAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.Contains(result, meal => meal.Id == 1 && meal.Name == "Meal1");
        Assert.Contains(result, meal => meal.Id == 2 && meal.Name == "Meal2");
    }
    [Fact]
    public async Task GetAll_ReturnsEmptyListOfMealsForNoMealsAdded()
    {
        // Arrange
        var repository = new MealRepository(_context);
        await repository.SaveAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.Empty(result);
    }
    [Fact]
    public async Task GetById_ReturnsEntityWhenExists()
    {
        // Arrange
        var repository = new MealRepository(_context);
        var expectedEntity = new Meal { Id = 1, Name = "Meal1" };
        await repository.AddAsync(expectedEntity);
        await repository.SaveAsync();

        // Act
        var result = await repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedEntity.Id, result.Id);
        Assert.Equal(expectedEntity.Name, result.Name);
    }

    [Fact]
    public async Task GetById_ReturnsNullWhenEntityDoesNotExist()
    {
        // Arrange
        var repository = new MealRepository(_context);
        // Act
        var entity = await repository.GetByIdAsync(1);
        // Assert
        Assert.Null(entity);
    }

    [Fact]
    public async Task Add_AddsEntityToDatabase()
    {
        // Arrange
        var repository = new MealRepository(_context);
        var entity = new Meal { Id = 1, Name = "Meal1" };

        // Act
        await repository.AddAsync(entity);
        await repository.SaveAsync();

        // Assert
        var result = _context.Set<Meal>().FirstOrDefault(e => e.Id == 1);
        Assert.NotNull(result);
        Assert.Equal(entity.Name, result.Name);
    }

    [Fact]
    public async Task Add_ThrowsEntityNotFoundExceptionWhenEntityIsNull()
    {
        // Arrange
        var repository = new MealRepository(_context);
        Meal entity = null!;

        // Act & Assert
        await AssertThrowsEntityNotFoundExceptionAsync(repository.AddAsync(entity));
    }

    [Fact]
    public async Task Update_UpdatesEntityToDatabase()
    {
        // Arrange
        var repository = new MealRepository(_context);
        var entity = new Meal { Id = 1, Name = "Meal1" };
        await repository.AddAsync(entity);
        await repository.SaveAsync();

        var updatedEntity = new Meal { Id = 1, Name = "NewIngredient1" };

        // Act

        await repository.UpdateAsync(updatedEntity, 1);
        await repository.SaveAsync();

        // Assert

        var result = _context.Set<Meal>().FirstOrDefault(e => e.Id == 1);
        Assert.NotNull(result);
        Assert.Equal(updatedEntity.Name, result.Name);
    }

    [Fact]
    public async Task Update_UpdatesEntityAndIngredientsToDatabase()
    {
        // Arrange
        var ingredientRepository = new Repository<Ingredient>(_context);
        var ingerdintToAdd = new Ingredient { Id = 1, Name = "name1" };
        await ingredientRepository.AddAsync(ingerdintToAdd);
        await ingredientRepository.SaveAsync();

        var repository = new MealRepository(_context);
        var entity = new Meal { Id = 1, Name = "Meal1" };
        await repository.AddAsync(entity);
        await repository.SaveAsync();

        var updatedEntity = new Meal
        {
            Id = 1,
            Name = "NewIngredient1",
            Ingredients = new List<Ingredient>
            {
                new Ingredient{ Id =1, Name = "Dont care"}
            }
        };

        // Act

        await repository.UpdateAsync(updatedEntity, 1);
        await repository.SaveAsync();

        // Assert

        var result = _context.Set<Meal>().FirstOrDefault(e => e.Id == 1);
        Assert.NotNull(result);
        Assert.Equal(updatedEntity.Name, result.Name);
        Assert.NotNull(result.Ingredients);
        Assert.Equal(updatedEntity.Ingredients[0].Id, result.Ingredients[0].Id);
    }

    [Fact]
    public async Task UpdateAsync_ThrowsArgumentExceptionWhenIdMismatch()
    {
        // Arrange
        var repository = new MealRepository(_context);
        var entity = new Meal { Id = 1, Name = "Meal1" };
        await repository.AddAsync(entity);
        await repository.SaveAsync();

        var updatedEntity = new Meal { Id = 2, Name = "NewMeal" }; // Mismatched Id

        // Act + Assert
        AssertThrowsInvalidOperationExceptionAsync(repository.UpdateAsync(updatedEntity, 1));
    }

    [Fact]
    public async Task Save_PersistsChangesToDatabase()
    {
        // Arrange
        var repository = new MealRepository(_context);
        var entity = new Meal { Id = 1, Name = "Meal1" };
        await repository.AddAsync(entity);

        // Act
        await repository.SaveAsync();

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
        await repository.AddAsync(new Meal { Id = 1, Name = "Meal1" });
        await repository.AddAsync(new Meal { Id = 2, Name = "Meal2" });
        await repository.AddAsync(new Meal { Id = 3, Name = "Meal3" });
        await repository.SaveAsync();

        // Act
        var result = await repository.GetInIdRangeAsync(2, 3);

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
        await repository.AddAsync(new Meal { Id = 1, Name = "Meal1" });
        await repository.AddAsync(new Meal { Id = 2, Name = "Meal2" });
        await repository.SaveAsync();

        // Act
        var result = await repository.GetInIdRangeAsync(3, 4);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task Delete_DeletesExistingEntity()
    {
        // Arrange
        var repository = new MealRepository(_context);
        var entity = new Meal { Id = 1, Name = "Meal" };
        await repository.AddAsync(entity);
        await repository.SaveAsync();

        // Act
        await repository.DeleteAsync(1);
        await repository.SaveAsync();
        // Assert
        var result = _context.Set<Meal>().FirstOrDefault(e => e.Id == 1);
        Assert.Null(result);
    }
    [Fact]
    public async Task Delete_ThrowsEntityNotFoundExceptionWhileDeletesNotExistingEntity()
    {
        // Arrange
        var repository = new MealRepository(_context);

        // Act
        // Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(
           async () => await repository.DeleteAsync(1));
    }

    [Fact]
    public async Task GetById_ReturnsEntityWithIncludesWhenExists()
    {
        // Arrange
        var ingredientRepository = new Repository<Ingredient>(_context);

        var ingredientEntity = new Ingredient()
        {
            Id = 1,
            Name = "name",
        };

        await ingredientRepository.AddAsync(ingredientEntity);
        await ingredientRepository.SaveAsync();

        var mealRepository = new MealRepository(_context);
        var expectedEntity = new Meal
        {
            Id = 1,
            Ingredients = new List<Ingredient> { ingredientEntity }
        };
        await mealRepository.AddAsync(expectedEntity);
        await mealRepository.SaveAsync();

        string includesString = "Ingredients";

        // Act
        var result = await mealRepository.GetByIdAsync(expectedEntity.Id, includesString);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedEntity.Id, result.Id);
        Assert.NotNull(result.Ingredients);
        Assert.Equal(result.Ingredients.Count, 1);
        Assert.Equal(expectedEntity.Ingredients[0], ingredientEntity);
    }

    [Fact]
    public async Task GetById_ThrowsExeptionWhenInludeStringIsInvalidExists()
    {
        // Arrange
        var ingredientRepository = new Repository<Ingredient>(_context);

        var ingredientEntity = new Ingredient()
        {
            Id = 1,
            Name = "name",
        };

        await ingredientRepository.AddAsync(ingredientEntity);
        await ingredientRepository.SaveAsync();

        var mealRepository = new MealRepository(_context);
        var expectedEntity = new Meal
        {
            Id = 1,
            Ingredients = new List<Ingredient> { ingredientEntity }
        };
        await mealRepository.AddAsync(expectedEntity);
        await mealRepository.SaveAsync();


        string includesString = "InvalidStiring";

        // Act + Assert

        AssertThrowsInvalidOperationExceptionAsync(mealRepository.GetByIdAsync(expectedEntity.Id, includesString));
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    private async Task AssertThrowsEntityNotFoundExceptionAsync(Task task)
    {
        await Assert.ThrowsAsync<EntityNotFoundException>(
            async () => await task);
    }
    private async Task AssertThrowsInvalidOperationExceptionAsync(Task task)
    {
        await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await task);
    }
}
