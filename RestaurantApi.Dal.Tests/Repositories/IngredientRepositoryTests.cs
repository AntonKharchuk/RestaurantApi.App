
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

using RestaurantApi.Dal.Exeptions;
using RestaurantApi.Dal.Models;
using RestaurantApi.Dal.Repositories;

using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace RestaurantApi.Dal.Tests.Repositories;

public class IngredientRepositoryTests : IDisposable
{
    private readonly DbContextOptions<RestaurantDbContext> _options;
    private readonly RestaurantDbContext _context;

    public IngredientRepositoryTests()
    {
        // Use an in-memory database for testing
        _options = new DbContextOptionsBuilder<RestaurantDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDatabase_{System.Guid.NewGuid()}")
            .Options;

        _context = new RestaurantDbContext(_options);
    }
    [Fact]
    public async Task GetAll_ReturnsCorrectNumOfIngredients()
    {
        // Arrange
        var repository = new Repository<Ingredient>(_context);
        await repository.AddAsync(new Ingredient { Id = 1, Name = "Ingredient1" });
        await repository.AddAsync(new Ingredient { Id = 2, Name = "Ingredient2" });
        await repository.SaveAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());

    }
    [Fact]
    public async Task GetAll_ReturnsCorrectIngredientsIngredients()
    {
        // Arrange
        var repository = new Repository<Ingredient>(_context);
        await repository.AddAsync(new Ingredient { Id = 1, Name = "Ingredient1" });
        await repository.AddAsync(new Ingredient { Id = 2, Name = "Ingredient2" });
        await repository.SaveAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.Contains(result, ingredient => ingredient.Id == 1 && ingredient.Name == "Ingredient1");
        Assert.Contains(result, ingredient => ingredient.Id == 2 && ingredient.Name == "Ingredient2");
    }
    [Fact]
    public async Task GetAll_ReturnsEmptyListOfIngredientsForNoInredientsAdded()
    {
        // Arrange
        var repository = new Repository<Ingredient>(_context);
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
        var repository = new Repository<Ingredient>(_context);
        var expectedEntity = new Ingredient { Id = 1, Name = "Ingredient1" };
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
        var repository = new Repository<Ingredient>(_context);

        // Act
        var result = await repository.GetByIdAsync(1);
        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Add_AddsEntityToDatabase()
    {
        // Arrange
        var repository = new Repository<Ingredient>(_context);
        var entity = new Ingredient { Id = 1, Name = "Ingredient1" };

        // Act
        await repository.AddAsync(entity);
        await repository.SaveAsync();

        // Assert
        var result = _context.Set<Ingredient>().FirstOrDefault(e => e.Id == 1);
        Assert.NotNull(result);
        Assert.Equal(entity.Name, result.Name);
    }
    [Fact]
    public async Task Add_ThrowsExeptionWhileAddingSeveralEntitiesWithSameIdToDatabase()
    {
        // Arrange
        var repository = new Repository<Ingredient>(_context);
        var entity = new Ingredient { Id = 1, Name = "Ingredient1" };
        var entity1 = new Ingredient { Id = 1, Name = "Ingredient1New" };

        // Act
        await repository.AddAsync(entity);
        var act = async () => await repository.AddAsync(entity1);

        // Assert
        Assert.ThrowsAsync<InvalidOperationException>(act);
        //var exeption = Record.Exception(act);

        //Assert.Equal(exeption, new InvalidOperationException());
    }
    [Fact]
    public async Task Add_ThrowsExeptionWhileAddingEntityWithIdBetweenExistedIds()
    {
        // Arrange
        var repository = new Repository<Ingredient>(_context);
        var entity1 = new Ingredient { Id = 1, Name = "Ingredient1" };
        var entity3 = new Ingredient { Id = 3, Name = "Ingredient3" };
        var entity2 = new Ingredient { Id = 2, Name = "Ingredient2New" };
        await repository.AddAsync(entity1);
        await repository.AddAsync(entity3);

        // Act
        var act = async () => await repository.AddAsync(entity2);

        // Assert
        Assert.ThrowsAsync<InvalidOperationException>(act);
        //var exeption = Record.Exception(act);

        //Assert.Equal(exeption, new InvalidOperationException());
    }

    [Fact]
    public async Task Add_ThrowsEntityNotFoundExceptionWhenEntityIsNull()
    {
        // Arrange
        var repository = new Repository<Ingredient>(_context);
        Ingredient entity = null!;

        // Act & Assert
        await AssertThrowsEntityNotFoundExceptionAsync(repository.AddAsync(entity));
    }

    [Fact]
    public async Task Update_UpdatesEntityToDatabase()
    {
        // Arrange
        var repository = new Repository<Ingredient>(_context);
        var entity = new Ingredient { Id = 1, Name = "Ingredient1" };
        await repository.AddAsync(entity);
        await repository.SaveAsync();

        var updatedEntity = new Ingredient { Id = 1, Name = "NewIngredient1" };

        // Act

        await repository.UpdateAsync(updatedEntity, 1);
        await repository.SaveAsync();

        // Assert

        var result = _context.Set<Ingredient>().FirstOrDefault(e => e.Id == 1);
        Assert.NotNull(result);
        Assert.Equal(updatedEntity.Name, result.Name);
    }
    [Fact]
    public async Task UpdateAsync_ThrowsArgumentExceptionWhenIdMismatch()
    {
        // Arrange
        var repository = new Repository<Ingredient>(_context);
        var entity = new Ingredient { Id = 1, Name = "Ingredient1" };
        await repository.AddAsync(entity);
        await repository.SaveAsync();

        var updatedEntity = new Ingredient { Id = 2, Name = "NewIngredient" }; // Mismatched Id

        // Act + Assert
        await Assert.ThrowsAsync<ArgumentException>(
            async () => await repository.UpdateAsync(updatedEntity, 1));
    }


    [Fact]
    public async Task Save_PersistsChangesToDatabase()
    {
        // Arrange
        var repository = new Repository<Ingredient>(_context);
        var entity = new Ingredient { Id = 1, Name = "Ingredient1" };
        await repository.AddAsync(entity);

        // Act
        await repository.SaveAsync();

        // Assert
        var result = _context.Set<Ingredient>().FirstOrDefault(e => e.Id == 1);
        Assert.NotNull(result);
        Assert.Equal(entity.Name, result.Name);
    }
    [Fact]
    public async Task Delete_DeletesExistingEntity()
    {
        // Arrange
        var repository = new Repository<Ingredient>(_context);
        var entity = new Ingredient { Id = 1, Name = "Ingredient1" };
        await repository.AddAsync(entity);
        await repository.SaveAsync();

        // Act
        await repository.DeleteAsync(1);
        await repository.SaveAsync();
        // Assert
        var result = _context.Set<Ingredient>().FirstOrDefault(e => e.Id == 1);
        Assert.Null(result);
    }
    [Fact]
    public async Task Delete_ThrowsEntityNotFoundExceptionWhileDeletesNotExistingEntity()
    {
        // Arrange
        var repository = new Repository<Ingredient>(_context);

        // Act
        // Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(
           async () => await repository.DeleteAsync(1));
    }
    [Fact]
    public async Task Add_CreatesItemAfterDeletingOne()
    {
        // Arrange
        var repository = new Repository<Ingredient>(_context);
        var entity1 = new Ingredient { Id = 1, Name = "Ingredient1" };
        var entity2 = new Ingredient { Id = 2, Name = "Ingredient2" };
        await repository.AddAsync(entity1);
        await repository.AddAsync(entity2);
        await repository.SaveAsync();

        // Act
        var entity1New = new Ingredient { Id = 1, Name = "Ingredient1" };
        await repository.DeleteAsync(1);
        await repository.SaveAsync();

        await repository.AddAsync(entity1New);
        await repository.SaveAsync();

        var addedItem = await repository.GetByIdAsync(1);
        // Assert
        Assert.Equal(addedItem.Name, entity1New.Name);
    }

    [Fact]
    public async Task Add_AddsBetweenTwoIds()
    {
        // Arrange
        var repository = new Repository<Ingredient>(_context);
        var entity1 = new Ingredient { Id = 1, Name = "Ingredient1" };
        var entity3 = new Ingredient { Id = 3, Name = "Ingredient3" };
        await repository.AddAsync(entity1);
        await repository.AddAsync(entity3);
        await repository.SaveAsync();
        var entity = new Ingredient { Name = "Ingredient4" };
        await repository.AddAsync(entity);
        await repository.SaveAsync();


        // Act
        var entity2New = new Ingredient { Id = 2, Name = "Ingredient2" };
        await repository.AddAsync(entity2New);
        await repository.SaveAsync();

        var addedItem = await repository.GetByIdAsync(2);
        // Assert
        Assert.Equal(addedItem.Name, entity2New.Name);
    }

    [Fact]
    public async Task Add_AddsItemWithIdEqualsIncrementedMaxId()
    {
        // Arrange
        var repository = new Repository<Ingredient>(_context);
        var entity1 = new Ingredient { Id = 1, Name = "Ingredient1" };
        var entity3 = new Ingredient { Id = 3, Name = "Ingredient3" };
        await repository.AddAsync(entity1);
        await repository.AddAsync(entity3);
        await repository.SaveAsync();

        // Act
        var entity = new Ingredient { Name = "Ingredient4" };
        await repository.AddAsync(entity);
        await repository.SaveAsync();

        var addedItem = await repository.GetByIdAsync(4);
        // Assert
        Assert.Equal(addedItem.Name, entity.Name);
        Assert.Equal(addedItem.Id, entity3.Id + 1);
    }




    public void Dispose()
    {
        // Clean up the in-memory database after each test
        _context.Dispose();
    }

    private async Task AssertThrowsEntityNotFoundExceptionAsync(Task task)
    {
        await Assert.ThrowsAsync<EntityNotFoundException>(
            async () => await task);
    }

}
