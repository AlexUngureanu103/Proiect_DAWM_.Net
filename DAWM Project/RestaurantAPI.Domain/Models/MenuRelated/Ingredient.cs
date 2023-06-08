namespace RestaurantAPI.Domain.Models.MenuRelated
{
    public class Ingredient : BaseEntity
    {
        public string Name { get; set; }

        public double TotalWeight { get; set; }
    }
}
