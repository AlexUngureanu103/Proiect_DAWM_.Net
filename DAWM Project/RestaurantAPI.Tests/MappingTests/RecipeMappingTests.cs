using RestaurantAPI.Domain.Dtos.RecipeDtos;
using RestaurantAPI.Domain.Mapping;

namespace RestaurantAPI.Tests.MappingTests
{
    [TestClass]
    public class RecipeMappingTests
    {
        private CreateOrUpdateRecipe recipeData;

        [TestInitialize]
        public void TestInitialize()
        {
            recipeData = new CreateOrUpdateRecipe
            {
                Name = "test",
                DishesTypeId = 1,
                Price = 21,
                DishesType = null
            };
        }

        [TestCleanup]
        public void TestCleanup()
        {
            recipeData = null;
        }

        [TestMethod]
        public void MapToUser_WhenUserIsNull_ReturnNull()
        {
            var ingredient = RecipeMapping.MapToRecipe(null);

            Assert.IsNull(ingredient, "Ingredient should be null when dto is null");
        }

        [TestMethod]
        public void MapToUser_WhenUserIsNull_ReturnUser()
        {
            var ingredient = RecipeMapping.MapToRecipe(recipeData);

            Assert.IsNotNull(ingredient, "User shouldn't be null when dto is null");

            Assert.AreEqual(ingredient.Name, recipeData.Name, "Resulted ingredient is not the same ");
            Assert.AreEqual(ingredient.Price, recipeData.Price, "Resulted ingredient is not the same ");
            Assert.AreEqual(ingredient.DishesType, recipeData.DishesType, "Resulted ingredient is not the same ");
            Assert.AreEqual(ingredient.DishesTypeId, recipeData.DishesTypeId, "Resulted ingredient is not the same ");
        }
    }
}
