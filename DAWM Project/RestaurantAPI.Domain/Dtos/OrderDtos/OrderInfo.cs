using RestaurantAPI.Domain.Dtos.MenuDtos;
using RestaurantAPI.Domain.Dtos.UserDtos;

namespace RestaurantAPI.Domain.Dtos.OrderDtos
{
    public class OrderInfo
    {
        public int OrderId { get; set; }

        public UserPublicData User { get; set; }

        public float Price { get; set; }

        public List<MenuOrderInfo> OrderedMenus { get; set; }

        public List<int> OrderSingleItem { get; set; }
    }
}
