namespace RestaurantAPI.Domain.Models.MenuRelated
{
    public class RecipeIngredient : BaseEntity
    {
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }

        public double Weight { get; set; }
    }
}
