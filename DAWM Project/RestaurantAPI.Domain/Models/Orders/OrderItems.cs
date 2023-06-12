using RestaurantAPI.Domain.Models.MenuRelated;

namespace RestaurantAPI.Domain.Models.Orders
{
    public class OrderItems : BaseEntity
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int MenuId { get; set; }
        public Menu Menu { get; set; }
    }
}
