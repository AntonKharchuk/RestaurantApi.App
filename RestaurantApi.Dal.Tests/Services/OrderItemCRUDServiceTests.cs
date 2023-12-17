
using Microsoft.EntityFrameworkCore;

using RestaurantApi.Dal.Models;
using RestaurantApi.Dal.Repositories;
using RestaurantApi.Dal.Services;

namespace RestaurantApi.Dal.Tests.Services;

public class OrderItemCRUDServiceTests
{
    private RestaurantDbContext _context;
    private OrderItemRepository _repository;
    private OrderItemCRUDService _testService;

    public OrderItemCRUDServiceTests()
    {
        // Use an in-memory database for testing
        var options = new DbContextOptionsBuilder<RestaurantDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDatabase_{System.Guid.NewGuid()}")
            .Options;

        _context = new RestaurantDbContext(options);

        _repository = new OrderItemRepository(_context);

        _testService = new OrderItemCRUDService(_repository);
    }

    [Fact]
    public async Task GetItemByOrderIdAsync_ShouldReturnItemsByOrderIdWithIncludes()
    {
        var orderRepository = new Repository<Order>(_context);
        await orderRepository.AddAsync(new Order
        {
            Id = 1,
            CustomerName = "Test"
        });
        await orderRepository.SaveAsync();

        // Arrange: Add test data
        var orderItem1 = new OrderItem { Id = 1, OrderId = 1, Coment = "Test1" };
        var orderItem2 = new OrderItem { Id = 2, OrderId = 1, Coment = "Test2" };
        var orderItem3 = new OrderItem { Id = 3, OrderId = 2, Coment = "Test3" };
        await _repository.AddAsync(orderItem1);
        await _repository.AddAsync(orderItem2);
        await _repository.AddAsync(orderItem3);
        await _repository.SaveAsync();

        // Act
        var result = await _testService.GetItemByOrderIdAsync(1, "Order");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());

        // Check that the included property (Product) is loaded
        var firstOrderItem = result.First();
        Assert.NotNull(firstOrderItem.Order);
    }

    [Fact]
    public async Task GetItemByOrderIdAsync_ShouldReturnItemsByOrderIdWithoutIncludes()
    {
        // Arrange: Add test data
        var orderItem1 = new OrderItem { Id = 1, OrderId = 1, Coment = "Test1" };
        var orderItem2 = new OrderItem { Id = 2, OrderId = 1, Coment = "Test2" };  
        var orderItem3 = new OrderItem { Id = 3, OrderId = 2, Coment = "Test3" };
        await _repository.AddAsync(orderItem1);
        await _repository.AddAsync(orderItem2);
        await _repository.AddAsync(orderItem3);
        await _repository.SaveAsync();

        // Act
        var result = await _testService.GetItemByOrderIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());

        // Check that the included property (Product) is not loaded
        var firstOrderItem = result.First();
        Assert.Null(firstOrderItem.Order);
    }

    [Fact]
    public async Task GetItemByOrderIdAsync_ShouldReturnEmptyListForNonExistingOrderId()
    {
        // Act
        var result = await _testService.GetItemByOrderIdAsync(999);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

}
