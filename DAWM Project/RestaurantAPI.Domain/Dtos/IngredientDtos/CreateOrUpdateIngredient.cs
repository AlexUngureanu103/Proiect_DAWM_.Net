namespace RestaurantAPI.Domain.Dtos.IngredientDtos
{
    public class CreateOrUpdateIngredient
    {
        public string Name { get; set; }

        public double TotalWeight { get; set; }
    }
}
