using RestaurantAPI.Domain.Models.Users;

namespace RestaurantAPI.Domain.Dtos.OrderDtos
{
    public class CreateOrUpdateOrder
    {
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
