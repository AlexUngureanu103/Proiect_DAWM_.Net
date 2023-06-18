using RestaurantAPI.Domain.Dtos.OrderDtos;
using RestaurantAPI.Domain.Dtos.RecipeDtos;
using RestaurantAPI.Domain.Models.Orders;
using RestaurantAPI.Domain.Models.Users;

namespace RestaurantAPI.Domain.Mapping
{
    public static class OrderMapping
    {
        public static Order MapToOrder(CreateOrUpdateOrder order)
        {
            if (order == null)
                return null;

            return new Order
            {
                UserId = order.UserId,
                User = order.User,
                OrderItems = new(),
                OrderSingleItems = new(),
                OrderDate = DateTime.Now
            };
        }

        public static OrderInfo MapToOrderInfos(Order order)
        {
            if (order == null)
                return null;

            return new OrderInfo
            {
                OrderId = order.Id,
                User = UserMapping.MapToUserPublicData(order.User),
                Price = order.OrderItems.Sum((order) => order.Menu.Price),
                OrderedMenus = order.OrderItems.Select(orderItems => orderItems.Menu).ToList(),
                OrderSingleItem = order.OrderSingleItems.Select(orderSingleItems => orderSingleItems.RecipieId).ToList()
            };
        }
    }
}
