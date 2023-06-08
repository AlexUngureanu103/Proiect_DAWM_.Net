namespace RestaurantAPI.Domain.Models.MenuRelated
{
    public class Recipe : BaseEntity
    {
        public string Name { get; set; }

        public float Price { get; set; }

        public int DishesTypeId { get; set; }
        public DishesType DishesType { get; set; }
    }
}
