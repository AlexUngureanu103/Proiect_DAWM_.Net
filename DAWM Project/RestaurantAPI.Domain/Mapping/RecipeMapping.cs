using RestaurantAPI.Domain.Dtos.RecipeDtos;
using RestaurantAPI.Domain.Models.MenuRelated;

namespace RestaurantAPI.Domain.Mapping
{
    public static class RecipeMapping
    {
        public static Recipe MapToRecipe(CreateOrUpdateRecipe recipe)
        {
            if (recipe == null)
                return null;

            return new Recipe
            {
                DishesTypeId = recipe.DishesTypeId,
                Name = recipe.Name,
                Price = recipe.Price,
                ImageUrl = recipe.ImageUrl
            };
        }

        public static RecipeInfo MapToRecipeInfo(Recipe recipe)
        {
            if (recipe == null)
                return null;
            return new RecipeInfo
            {
                Name = recipe.Name,
                Price = recipe.Price,
                DishesTypeId = recipe.DishesTypeId,
                ImageUrl = recipe.ImageUrl,
                IngredientIdAndWeight = recipe.Ingredients.Select(r => new IngredientAndWeight { IngredientId = r.IngredientId, Weight = r.Weight }).ToList()
            };
        }
    }
}
