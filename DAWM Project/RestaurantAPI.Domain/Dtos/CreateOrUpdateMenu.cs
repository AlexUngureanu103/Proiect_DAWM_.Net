using RestaurantAPI.Domain.Models.MenuRelated;

namespace RestaurantAPI.Domain.Dtos
{
    public class CreateOrUpdateMenu
    {
        public string Name { get; set; }

        public float Price { get; set; }
    }
}
