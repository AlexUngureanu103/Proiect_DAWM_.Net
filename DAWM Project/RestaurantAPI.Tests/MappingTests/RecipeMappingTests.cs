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
                Price = 21
            };
        }

        [TestCleanup]
        public void TestCleanup()
        {
            recipeData = null;
        }

        [TestMethod]
        public void MapToRecipe_WhenRecipeIsNull_ReturnNull()
        {
            var recipe = RecipeMapping.MapToRecipe(null);

            Assert.IsNull(recipe, "Recipe should be null when dto is null");
        }

        [TestMethod]
        public void MapToRecipe_WhenRecipeIsNull_ReturnRecipe()
        {
            var recipe = RecipeMapping.MapToRecipe(recipeData);

            Assert.IsNotNull(recipe, "Recipe shouldn't be null when dto is null");

            Assert.AreEqual(recipe.Name, recipeData.Name, "Resulted recipe is not the same ");
            Assert.AreEqual(recipe.Price, recipeData.Price, "Resulted recipe is not the same ");
            Assert.AreEqual(recipe.DishesTypeId, recipeData.DishesTypeId, "Resulted recipe is not the same ");
        }
    }
}
