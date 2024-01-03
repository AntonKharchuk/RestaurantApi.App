
using Moq;

using RestaurantApi.Dal.Models;
using RestaurantApi.Business.Services;

namespace RestaurantApi.Dal.Tests.Services;

public class OrdersServiceTests
{
    [Fact]
    public void Constructor_InitializesProperties()
    {
        // Arrange
        var orderServiceMock = new Mock<ICRUDService<Order>>();
        var orderItemServiceMock = new Mock<IOrderItemCRUDService>();

        // Act
        var ordersService = new OrdersService(
            orderServiceMock.Object,
            orderItemServiceMock.Object
        );

        // Assert
        Assert.NotNull(ordersService.OrderService);
        Assert.NotNull(ordersService.OrderItemService);
    }
}
