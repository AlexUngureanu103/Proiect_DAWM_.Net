using RestaurantAPI.Domain.Dtos.RecipeDtos;
using RestaurantAPI.Domain.Mapping;
using RestaurantAPI.Domain.Models.MenuRelated;

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
                ImageUrl = "test"
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
            Assert.AreEqual(recipe.ImageUrl, recipeData.ImageUrl, "Resulted recipe is not the same ");
        }

        [TestMethod]
        public void MapToRecipeInfo_WhenRecipeIsNull_ReturnNull()
        {
            var recipe = RecipeMapping.MapToRecipeInfo(null);

            Assert.IsNull(recipe, "Recipe should be null when dto is null");
        }

        [TestMethod]
        public void MapToRecipeInfo_WhenRecipeIsNull_ReturnRecipe()
        {
            Recipe recipe = new Recipe
            {
                DishesTypeId = 1,
                Id = 1,
                ImageUrl = "test",
                Ingredients = new(),
                Name = "Test Name",
                Price = 12
            };
            var recipeInfo = RecipeMapping.MapToRecipeInfo(recipe);

            Assert.IsNotNull(recipe, "Recipe shouldn't be null when dto is null");

            Assert.AreEqual(recipe.Name, recipeInfo.Name, "Resulted recipe Info is not the same ");
            Assert.AreEqual(recipe.Price, recipeInfo.Price, "Resulted recipe Info is not the same ");
            Assert.AreEqual(recipe.DishesTypeId, recipeInfo.DishesTypeId, "Resulted recipe Info is not the same ");
            Assert.AreEqual(recipe.ImageUrl, recipeInfo.ImageUrl, "Resulted recipe Info is not the same ");
        }
    }
}
