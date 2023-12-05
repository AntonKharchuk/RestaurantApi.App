
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

using RestaurantApi.Dal.Models;
using RestaurantApi.Dal.Repositories;

namespace RestaurantApi.Dal.Tests.Repositories
{
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
            await repository.Add(new Ingredient { Id = 1, Name = "Ingredient1" });
            await repository.Add(new Ingredient { Id = 2, Name = "Ingredient2" });
            await repository.Save();

            // Act
            var result = await repository.GetAll();

            // Assert
            Assert.Equal(2, result.Count());

        }
        [Fact]
        public async Task GetAll_ReturnsCorrectIngredientsIngredients()
        {
            // Arrange
            var repository = new Repository<Ingredient>(_context);
            await repository.Add(new Ingredient { Id = 1, Name = "Ingredient1" });
            await repository.Add(new Ingredient { Id = 2, Name = "Ingredient2" });
            await repository.Save();

            // Act
            var result = await repository.GetAll();

            // Assert
            Assert.Contains(result, ingredient => ingredient.Id == 1 && ingredient.Name == "Ingredient1");
            Assert.Contains(result, ingredient => ingredient.Id == 2 && ingredient.Name == "Ingredient2");
        }
        [Fact]
        public async Task GetAll_ReturnsEmptyListOfIngredientsForNoInredientsAdded()
        {
            // Arrange
            var repository = new Repository<Ingredient>(_context);
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
            var repository = new Repository<Ingredient>(_context);
            var expectedEntity = new Ingredient { Id = 1, Name = "Ingredient1" };
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
            var repository = new Repository<Ingredient>(_context);

            // Act & Assert
            await AssertThrowsArgumentNullExceptionAsync(repository.GetById(1));
        }

        [Fact]
        public async Task GetById_ThrowsExceptionWhenEntityIsNull()
        {
            // Arrange
            var repository = new Repository<Ingredient>(_context);
            await repository.Add(new Ingredient { Id = 1, Name = "Ingredient1" });
            await repository.Save();

            // Act & Assert
            await AssertThrowsArgumentNullExceptionAsync(repository.GetById(2));
        }

        [Fact]
        public async Task Add_AddsEntityToDatabase()
        {
            // Arrange
            var repository = new Repository<Ingredient>(_context);
            var entity = new Ingredient { Id = 1, Name = "Ingredient1" };

            // Act
            await repository.Add(entity);
            await repository.Save();

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
            await repository.Add(entity);
            await repository.Add(entity1);
            await repository.Save();

            // Assert
            //var exeption = Record.Exception(act);

            //Assert.Equal(exeption, new InvalidOperationException());
        }

        [Fact]
        public async Task Add_ThrowsArgumentNullExceptionWhenEntityIsNull()
        {
            // Arrange
            var repository = new Repository<Ingredient>(_context);
            Ingredient entity = null!;

            // Act & Assert
            await AssertThrowsArgumentNullExceptionAsync(repository.Add(entity));
        }

        [Fact]
        public async Task Update_UpdatesEntityToDatabase()
        {
            // Arrange
            var repository = new Repository<Ingredient>(_context);
            var entity = new Ingredient { Id = 1, Name = "Ingredient1" };
            await repository.Add(entity);
            await repository.Save();

            var updatedEntity = new Ingredient { Id = 1, Name = "NewIngredient1" };

            // Act

            await repository.Update(updatedEntity, 1);
            await repository.Save();

            // Assert

            var result = _context.Set<Ingredient>().FirstOrDefault(e => e.Id == 1);
            Assert.NotNull(result);
            Assert.Equal(updatedEntity.Name, result.Name);
        }

        //[Fact]
        //public async Task Update_ThrowsExceptionWhenIdAndIngredientIdMissmatch()
        //{
        //    // Arrange
        //    var repository = new Repository<Ingredient>(_context);
        //    var entity1 = new Ingredient { Id = 1, Name = "Ingredient1" };
        //    var entity2 = new Ingredient { Id = 2, Name = "Ingredient2" };
        //    await repository.Add(entity1);
        //    await repository.Add(entity2);
        //    await repository.Save();

        //    var updatedEntity = new Ingredient { Id = 1, Name = "NewIngredient1" };

        //    //Act 

        //    Action act = async () => await repository.Update(updatedEntity, 2);
        //    //assert

        //    var ex = Record.Exception(act);
        //}

        [Fact]
        public async Task Save_PersistsChangesToDatabase()
        {
            // Arrange
            var repository = new Repository<Ingredient>(_context);
            var entity = new Ingredient { Id = 1, Name = "Ingredient1" };
            await repository.Add(entity);

            // Act
            await repository.Save();

            // Assert
            var result = _context.Set<Ingredient>().FirstOrDefault(e => e.Id == 1);
            Assert.NotNull(result);
            Assert.Equal(entity.Name, result.Name);
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
