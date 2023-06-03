namespace RestaurantAPI.Domain.Models.MenuRelated
{
    public class MenuItem : BaseEntity
    {
        public int MenuId { get; set; }
        public Menu Menu { get; set; }

        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}
