using RestaurantAPI.Domain.Models.MenuRelated;

namespace RestaurantAPI.Domain.Dtos.MenuDtos
{
    public class CreateOrUpdateMenu
    {
        public string Name { get; set; }

        public float Price { get; set; }
    }
}
