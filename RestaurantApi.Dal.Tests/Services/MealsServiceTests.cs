using Xunit;
using Moq; // You may need to install the Moq NuGet package for mocking
using RestaurantApi.Dal.Services;
using RestaurantApi.Dal.Models;

public class MealsServiceTests
{
    [Fact]
    public void Constructor_InitializesProperties()
    {
        // Arrange
        var ingredientServiceMock = new Mock<ICRUDService<Ingredient>>();
        var mealServiceMock = new Mock<IMealCRUDService>();
        var portionServiceMock = new Mock<ICRUDService<Portion>>();
        var priceListServiceMock = new Mock<ICRUDService<PriceListItem>>();

        // Act
        var mealsService = new MealsService(
            ingredientServiceMock.Object,
            mealServiceMock.Object,
            portionServiceMock.Object,
            priceListServiceMock.Object
        );

        // Assert
        Assert.NotNull(mealsService.IngredientService);
        Assert.NotNull(mealsService.MealService);
        Assert.NotNull(mealsService.PortionService);
        Assert.NotNull(mealsService.PriceListService);
    }

}
