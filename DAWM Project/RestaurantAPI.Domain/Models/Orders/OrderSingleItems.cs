using RestaurantAPI.Domain.Models.MenuRelated;

namespace RestaurantAPI.Domain.Models.Orders
{
    public class OrderSingleItems : BaseEntity
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int RecipieId { get; set; }
        public Recipe Recipie { get; set; }

        public int Quantity { get; set; }
    }
}
