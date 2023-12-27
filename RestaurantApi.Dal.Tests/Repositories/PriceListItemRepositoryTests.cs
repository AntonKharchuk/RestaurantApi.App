
using Microsoft.EntityFrameworkCore;

using RestaurantApi.Dal.Exeptions;
using RestaurantApi.Dal.Models;
using RestaurantApi.Dal.Repositories;

namespace RestaurantApi.Dal.Tests.Repositories;

public class PriceListItemRepositoryTests : IDisposable
{
    private readonly DbContextOptions<RestaurantDbContext> _options;
    private readonly RestaurantDbContext _context;

    public PriceListItemRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<RestaurantDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDatabase_{System.Guid.NewGuid()}")
            .Options;

        _context = new RestaurantDbContext(_options);
    }

    [Fact]
    public async Task GetById_ReturnsEntityWithIncludesWhenExists()
    {
        // Arrange
        var portionRepository = new Repository<Portion>(_context);

        var portionEntity = new Portion()
        {
            Id = 1,
            Name = "name",
        };

        await portionRepository.AddAsync(portionEntity);
        await portionRepository.SaveAsync();

        var priceListItemRepository = new Repository<PriceListItem>(_context);
        var expectedEntity = new PriceListItem
        {
            Id = 1,
            Amount = 1,
            Price = 1,
            PortionId = portionEntity.Id,
        };
        await priceListItemRepository.AddAsync(expectedEntity);
        await priceListItemRepository.SaveAsync();

        string includesString = "Portion";

        // Act
        var result = await priceListItemRepository.GetByIdAsync(expectedEntity.Id, includesString);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedEntity.Id, result.Id);
        Assert.Equal(expectedEntity.Price, result.Price);
        Assert.Equal(expectedEntity.PortionId, portionEntity.Id);
        Assert.Equal(expectedEntity.Portion, portionEntity);
    }

    [Fact]
    public async Task GetById_ThrowsExeptionWhenInludeStringIsInvalidExists()
    {
        // Arrange
        var portionRepository = new Repository<Portion>(_context);

        var portionEntity = new Portion()
        {
            Id = 1,
            Name = "name",
        };

        await portionRepository.AddAsync(portionEntity);
        await portionRepository.SaveAsync();

        var priceListItemRepository = new Repository<PriceListItem>(_context);
        var expectedEntity = new PriceListItem
        {
            Id = 1,
            Amount = 1,
            Price = 1,
            PortionId = portionEntity.Id,
        };
        await priceListItemRepository.AddAsync(expectedEntity);
        await priceListItemRepository.SaveAsync();

        string includesString = "InvalidStiring";

        // Act + Assert

        AssertThrowsInvalidOperationExceptionAsync(priceListItemRepository.GetByIdAsync(expectedEntity.Id, includesString));
    }
    private async Task AssertThrowsInvalidOperationExceptionAsync(Task task)
    {
        await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await task);
    }
    public void Dispose()
    {
        _context.Dispose();
    }
}
