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

            var existingIngredientId = 20;

            // Act
            var response = await client.GetAsync($"/api/Meal/Ingredients/{existingIngredientId}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var ingredient = JsonConvert.DeserializeObject<Ingredient>(content);

            Assert.NotNull(ingredient);
        }

        [Fact]
        public async Task GetIngredientById_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var client = _factory.CreateClient();

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

            var newIngredient = new Ingredient
            {
                Name = "TestIngredient",
            };

            var json = JsonConvert.SerializeObject(newIngredient);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/Meal/Ingredients", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var createdIngredient = JsonConvert.DeserializeObject<Ingredient>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(createdIngredient);
            Assert.Equal(newIngredient.Name, createdIngredient.Name);

            Assert.NotNull(response.Headers.Location);
        }

        [Fact]
        public async Task UpdateIngredient_ReturnsSuccessStatusCode()
        {
            // Arrange
            var client = _factory.CreateClient();

            var existingIngredientId = 20;

            var updatedIngredient = new Ingredient
            {
                Id= existingIngredientId,
                Name = "UpdatedIngredient",
            };

            var json = JsonConvert.SerializeObject(updatedIngredient);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PutAsync($"/api/Meal/Ingredients?id={existingIngredientId}", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var updatedIngredientFromDb = await GetIngredientFromDatabase(client, existingIngredientId);


            Assert.NotNull(updatedIngredientFromDb);
            Assert.Equal(updatedIngredient.Name, updatedIngredientFromDb.Name);
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

            var existingIngredientId = lastIngredient.Id;

            // Act
            var response = await client.DeleteAsync($"/api/Meal/Ingredients/{existingIngredientId}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent);

            var ensureDeletedResponse = await client.GetAsync($"/api/Meal/Ingredients/{existingIngredientId}");

            Assert.Equal(ensureDeletedResponse.StatusCode, HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteIngredient_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var client = _factory.CreateClient();

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

            var content = await response.Content.ReadAsStringAsync();
            var ingredients = JsonConvert.DeserializeObject<List<Ingredient>>(content);
            return ingredients;
        }

    }
}