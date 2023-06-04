using RestaurantAPI.Domain.Models.MenuRelated;

namespace RestaurantAPI.Domain.Dtos.RecipeDtos
{
    public class CreateOrUpdateRecipe
    {
        public string Name { get; set; }

        public float Price { get; set; }

        public int DishesTypeId { get; set; }
        public DishesType DishesType { get; set; }
    }
}
