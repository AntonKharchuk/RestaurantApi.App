
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensibility;

using RestaurantApi.Dal.Models;
using RestaurantApi.Dal.Repositories;

namespace RestaurantApi.Dal.Tests.Repositories
{
    public class OrderItemRepositoryTests
    {
        private readonly DbContextOptions<RestaurantDbContext> _options;
        private readonly RestaurantDbContext _context;

        public OrderItemRepositoryTests()
        {
            // Use an in-memory database for testing
            _options = new DbContextOptionsBuilder<RestaurantDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDatabase_{System.Guid.NewGuid()}")
                .Options;

            _context = new RestaurantDbContext(_options);
        }
        [Fact]
        public async Task GetByOrderId_ReturnsEmptyListOfOrderItemsForNoOrderItemsAdded()
        {
            // Arrange
            var repository = new OrderItemRepository(_context);
            await repository.SaveAsync();

            // Act
            var result = await repository.GetByOrderId(1);

            // Assert
            Assert.Empty(result);
        }
        [Fact]
        public async Task GetByOrderId_ReturnsListOfOerderItemsThatMatchOrderId()
        {
            // Arrange

            var orderRepository = new Repository<Order>(_context);

            await orderRepository.AddAsync(new Order
            {
                Id = 1,
                CustomerName = "Test",
            });

            await orderRepository.SaveAsync();


            var repository = new OrderItemRepository(_context);


            var orderItemToAdd1 = new OrderItem()
            {
                Id =1,
                Coment = "TestComent1",
                OrderId =1,
            };
            var orderItemToAdd2 = new OrderItem()
            {
                Id = 2,
                Coment = "TestComent2",
                OrderId = 1,
            };
            await repository.AddAsync(orderItemToAdd1);
            await repository.AddAsync(orderItemToAdd2);
            await repository.SaveAsync();

            // Act
            var result = await repository.GetByOrderId(1);
            // Assert
            Assert.Equal(result.Count(), 2);
            Assert.Equal(result.First().OrderId, 1);
        }

        [Fact]
        public async Task GetByOrderId_ReturnsListOfOerderItemsThatMatchOrderIdAndIncludeData()
        {
            // Arrange

            var orderRepository = new Repository<Order>(_context);

            await orderRepository.AddAsync(new Order
            {
                Id = 1,
                CustomerName = "Test",
            });

            await orderRepository.SaveAsync();


            var repository = new OrderItemRepository(_context);


            var orderItemToAdd1 = new OrderItem()
            {
                Id = 1,
                Coment = "TestComent1",
                OrderId = 1,
            };
            var orderItemToAdd2 = new OrderItem()
            {
                Id = 2,
                Coment = "TestComent2",
                OrderId = 1,
            };
            await repository.AddAsync(orderItemToAdd1);
            await repository.AddAsync(orderItemToAdd2);
            await repository.SaveAsync();

            var includeString = "Order";

            // Act
            var result = await repository.GetByOrderId(1, includeString);
            // Assert
            Assert.Equal(result.Count(), 2);
            Assert.Equal(result.First().OrderId, 1);
            Assert.Equal(result.First().Order.Id, 1);
        }

        [Fact]
        public async Task GetByOrderId_ThrowExeptionWhenIncludeStringIsIncorrect()
        {
            // Arrange

            var orderRepository = new Repository<Order>(_context);

            await orderRepository.AddAsync(new Order
            {
                Id = 1,
                CustomerName = "Test",
            });

            await orderRepository.SaveAsync();


            var repository = new OrderItemRepository(_context);


            var orderItemToAdd1 = new OrderItem()
            {
                Id = 1,
                Coment = "TestComent1",
                OrderId = 1,
            };
            var orderItemToAdd2 = new OrderItem()
            {
                Id = 2,
                Coment = "TestComent2",
                OrderId = 1,
            };
            await repository.AddAsync(orderItemToAdd1);
            await repository.AddAsync(orderItemToAdd2);
            await repository.SaveAsync();

            var includeString = "IncorrctString";

            // Act + Assert
            await AssertThrowsInvalidOperationExceptionAsync(repository.GetByOrderId(1, includeString));
        }
        private async Task AssertThrowsInvalidOperationExceptionAsync(Task task)
        {
            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await task);
        }
    }
}
