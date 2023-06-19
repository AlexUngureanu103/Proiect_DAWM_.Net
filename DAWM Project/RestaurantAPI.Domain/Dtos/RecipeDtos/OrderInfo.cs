using RestaurantAPI.Domain.Dtos.UserDtos;
using RestaurantAPI.Domain.Models.MenuRelated;

namespace RestaurantAPI.Domain.Dtos.RecipeDtos
{
    public class OrderInfo
    {
        public int OrderId { get; set; }

        public UserPublicData User { get; set; }

        public float Price { get; set; }

        public List<Menu> OrderedMenus { get; set; }

        public List<int> OrderSingleItem { get; set; }
    }
}
