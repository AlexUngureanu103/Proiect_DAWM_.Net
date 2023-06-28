using RestaurantAPI.Domain.Dtos.IngredientDtos;
using RestaurantAPI.Domain.Mapping;

namespace RestaurantAPI.Tests.MappingTests
{
    [TestClass]
    public class IngredientsMappingTests
    {
        private CreateOrUpdateIngredient ingredientData;

        [TestInitialize]
        public void TestInitialize()
        {
            ingredientData = new CreateOrUpdateIngredient
            {
                Name = "test",
            };
        }

        [TestCleanup]
        public void TestCleanup()
        {
            ingredientData = null;
        }

        [TestMethod]
        public void MapToIngredient_WhenIngredientIsNull_ReturnNull()
        {
            var ingredient = IngredientMapping.MapToIngredient(null);

            Assert.IsNull(ingredient, "Ingredient should be null when dto is null");
        }

        [TestMethod]
        public void MapToIngredient_WhenIngredientIsNull_ReturnIngredient()
        {
            var ingredient = IngredientMapping.MapToIngredient(ingredientData);

            Assert.IsNotNull(ingredient, "Ingredient shouldn't be null when dto is null");

            Assert.AreEqual(ingredient.Name, ingredientData.Name, "Resulted ingredient is not the same ");

        }
    }
}
