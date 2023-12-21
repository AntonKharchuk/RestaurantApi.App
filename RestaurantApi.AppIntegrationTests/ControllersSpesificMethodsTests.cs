
using Newtonsoft.Json;

using RestaurantApi.App.Models;

using System.Net;

namespace RestaurantApi.AppIntegrationTests
{
    public class ControllersSpesificMethodsTests : IClassFixture<ApiWebApplicationFactory>
    {
        private readonly ApiWebApplicationFactory _factory;

        public ControllersSpesificMethodsTests()
        {
            _factory = new ApiWebApplicationFactory();
        }
        [Fact]
        public async Task Meal_MealsPagiantion_ReturnsSuccessStatusCode()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/Meal/Meals/Page/1");


            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task Meal_MealsPagiantion_ReturnsCorrectData()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/Meal/Meals/Page/1");


            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var meals = JsonConvert.DeserializeObject<List<Meal>>(content);

            Assert.NotNull(meals);
            Assert.NotEmpty(meals);
        }
        [Fact]
        public async Task Order_OrderItemByOrder_ReturnsSuccessStatusCode()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/Order/OrderItems?orderId=1");


            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var meals = JsonConvert.DeserializeObject<List<OrderItem>>(content);

            Assert.NotNull(meals);
            Assert.NotEmpty(meals);
        }

        [Fact]
        public async Task Order_OrderItemByOrder_ReturnsCorrectData()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/Order/OrderItems?orderId=1");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var meals = JsonConvert.DeserializeObject<List<OrderItem>>(content);

            Assert.NotNull(meals);
            Assert.NotEmpty(meals);
        }
    }
}
