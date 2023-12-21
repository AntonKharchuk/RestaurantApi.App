
using Newtonsoft.Json;

using RestaurantApi.App.Models;

using System.Net;

namespace RestaurantApi.AppIntegrationTests
{
    public class MealControllerMealsTest:IClassFixture<ApiWebApplicationFactory>
    {
        private readonly ApiWebApplicationFactory _factory;

        public MealControllerMealsTest()
        {
            _factory = new ApiWebApplicationFactory();
        }
        [Fact]
        public async Task GetAllMeals_IncludeQuery_ReturnsSuccessStatusCode()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/Meal/Meals?includePath=Ingredients");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task GetAllMeals_IncludeQuery_ReturnsCorrectData()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/Meal/Meals?includePath=Ingredients");


            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var meals = JsonConvert.DeserializeObject<List<Meal>>(content);

            Assert.NotNull(meals);
            Assert.NotEmpty(meals);
            Assert.NotNull(meals[0].Ingredients);
            Assert.NotEmpty(meals[0].Ingredients);
        }
    }
}
