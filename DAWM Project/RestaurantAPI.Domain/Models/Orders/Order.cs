using RestaurantAPI.Domain.Models.Users;

namespace RestaurantAPI.Domain.Models.Orders
{
    public class Order : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public List<OrderItems> OrderItems { get; set; }
    }
}
