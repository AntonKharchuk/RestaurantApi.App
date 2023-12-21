using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

using Newtonsoft.Json;

using RestaurantApi.App.Models;
using RestaurantApi.Dal;


namespace RestaurantApi.AppIntegrationTests
{
    internal class ApiWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                // Add the DbContext with the in-memory database
                services.AddDbContext<RestaurantDbContext>(options =>
                {
                    options.UseInMemoryDatabase(databaseName: $"TestDatabase_{System.Guid.NewGuid()}");
                });

                // Optionally, you can seed initial data for your tests
                SeedTestData(services);
            });
        }

        private void SeedTestData(IServiceCollection services)
        {
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var dbContext = serviceProvider.GetRequiredService<RestaurantDbContext>();

            }
        }
    }

    public class MealControllerIntegrationIngredientTests : IClassFixture<ApiWebApplicationFactory>
    {
        private readonly ApiWebApplicationFactory _factory;

        public MealControllerIntegrationIngredientTests()
        {
            _factory = new ApiWebApplicationFactory();
        }

        [Fact]
        public async Task GetAllIngredients_ReturnsSuccessStatusCode()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/Meal/Ingredients");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task GetAllIngredients_ReturnsCorrectData()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/Meal/Ingredients");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var ingredients = JsonConvert.DeserializeObject<IEnumerable<Ingredient>>(content);

            Assert.NotNull(ingredients);
            Assert.NotEmpty(ingredients);
        }

       
        [Fact]
        public async Task GetIngredientById_ExistingId_ReturnsCorrectData()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Replace 1 with an existing ingredient id in your test database
            var existingIngredientId = 20;

            // Act
            var response = await client.GetAsync($"/api/Meal/Ingredients/{existingIngredientId}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Deserialize the response content to check the data
            var content = await response.Content.ReadAsStringAsync();
            var ingredient = JsonConvert.DeserializeObject<Ingredient>(content);

            // Add your specific assertions for the returned data
            Assert.NotNull(ingredient);
            // You may want to add more specific assertions based on your actual data
            // For example, checking if certain properties have expected values.
        }

        [Fact]
        public async Task GetIngredientById_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Replace 999 with a non-existing ingredient id in your test database
            var nonExistingIngredientId = 999;

            // Act
            var response = await client.GetAsync($"/api/Meal/Ingredients/{nonExistingIngredientId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact]
        public async Task CreateIngredient_ReturnsSuccessStatusCode()
        {
            // Arrange
            var client = _factory.CreateClient();
            var newIngredient = new Ingredient
            {
                Id = 0,
                Name = "Test",
            };
            var json = JsonConvert.SerializeObject(newIngredient);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/Meal/Ingredients", content);

            var context = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task CreateIngredient_ReturnsSuccessStatusCodeAndCreatedIngredient()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Create a new Ingredient object for testing
            var newIngredient = new Ingredient
            {
                // Set the properties according to your test data
                Name = "TestIngredient",
                // Add other properties as needed
            };

            var json = JsonConvert.SerializeObject(newIngredient);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/Meal/Ingredients", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            // Deserialize the response content to check the created Ingredient
            var createdIngredient = JsonConvert.DeserializeObject<Ingredient>(await response.Content.ReadAsStringAsync());

            // Add your specific assertions for the created Ingredient
            Assert.NotNull(createdIngredient);
            Assert.Equal(newIngredient.Name, createdIngredient.Name);
            // Add other assertions based on your actual data

            // You may also want to check the Location header for the created resource
            Assert.NotNull(response.Headers.Location);
            // Add assertions for the Location header based on your application's expectations
        }

        [Fact]
        public async Task UpdateIngredient_ReturnsSuccessStatusCode()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Replace 1 with an existing ingredient id in your test database
            var existingIngredientId = 20;

            // Create an updated Ingredient object for testing
            var updatedIngredient = new Ingredient
            {
                // Set the properties according to your test data for the update
                Id= existingIngredientId,
                Name = "UpdatedIngredient",
                // Add other properties as needed
            };

            var json = JsonConvert.SerializeObject(updatedIngredient);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PutAsync($"/api/Meal/Ingredients?id={existingIngredientId}", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Optionally, you can retrieve the updated Ingredient from the database and assert its properties
            var updatedIngredientFromDb = await GetIngredientFromDatabase(client, existingIngredientId);


            // Add your specific assertions for the updated Ingredient
            Assert.NotNull(updatedIngredientFromDb);
            Assert.Equal(updatedIngredient.Name, updatedIngredientFromDb.Name);
            // Add other assertions based on your actual data
        }

        [Fact]
        public async Task DeleteIngredient_ExistingId_ReturnsSuccessStatusCode()
        {
            // Arrange
            var client = _factory.CreateClient();
            var ingredien = new Ingredient()
            {
                Id = 0 ,
                Name = "TestToDelete"
            };

            var IsCreated = await CreateIngredientInDatabase(client,ingredien);
            if (!IsCreated)
            {
                Assert.Fail("TestIngredientToDelete not created");
            }

           var ingredients =  await GetAllIngredientsFromDatabase(client);

            var lastIngredient = ingredients[ingredients.Count - 1];

            // Replace 1 with an existing ingredient id in your test database
            var existingIngredientId = lastIngredient.Id;

            // Act
            var response = await client.DeleteAsync($"/api/Meal/Ingredients/{existingIngredientId}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent);

            var ensureDeletedResponse = await client.GetAsync($"/api/Meal/Ingredients/{existingIngredientId}");

            Assert.Equal(ensureDeletedResponse.StatusCode, HttpStatusCode.NotFound); // Ensure the ingredient is deleted
        }

        [Fact]
        public async Task DeleteIngredient_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Replace 999 with a non-existing ingredient id in your test database
            var nonExistingIngredientId = 999;

            // Act
            var response = await client.DeleteAsync($"/api/Meal/Ingredients/{nonExistingIngredientId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        private async Task<bool> CreateIngredientInDatabase(HttpClient client, Ingredient ingredient)
        {
            var json = JsonConvert.SerializeObject(ingredient);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/Meal/Ingredients", content);
            response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
        }

        private async Task<Ingredient> GetIngredientFromDatabase( HttpClient client, int id)
        {

            var response = await client.GetAsync($"/api/Meal/Ingredients/{id}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Deserialize the response content to check the data
            var content = await response.Content.ReadAsStringAsync();
            var ingredient = JsonConvert.DeserializeObject<Ingredient>(content);
            return ingredient;  
        }

        private async Task<List<Ingredient>> GetAllIngredientsFromDatabase(HttpClient client)
        {

            var response = await client.GetAsync($"/api/Meal/Ingredients");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Deserialize the response content to check the data
            var content = await response.Content.ReadAsStringAsync();
            var ingredients = JsonConvert.DeserializeObject<List<Ingredient>>(content);
            return ingredients;
        }

    }
}