using RestaurantAPI.Domain.Dtos.OrderDtos;
using RestaurantAPI.Domain.Mapping;
using RestaurantAPI.Domain.Models.Orders;
using RestaurantAPI.Domain.Models.Users;

namespace RestaurantAPI.Tests.MappingTests
{
    [TestClass]
    public class OrderMappingTests
    {
        private CreateOrUpdateOrder orderData;
        private Order order;

        [TestInitialize]
        public void TestInitialize()
        {
            orderData = new CreateOrUpdateOrder
            {
                User = new User { Id = 1 },
                UserId = 1,
            };
            order = new Order
            {
                Id = 1,
                User = new User { Id = 1 },
                UserId = 1,
                OrderItems = new()
            };
        }

        [TestCleanup]
        public void TestCleanup()
        {
            orderData = null;
            order = null;
        }

        [TestMethod]
        public void MapToOrder_WhenOrderDataIsNull_ReturnNull()
        {
            var mappedOrder = OrderMapping.MapToOrder(null);

            Assert.IsNull(mappedOrder, "Order should be null when dto is null");
        }

        [TestMethod]
        public void MapToOrder_WhenOrderDataIsNotNull_ReturnOrder()
        {
            var mappedOrder = OrderMapping.MapToOrder(orderData);

            Assert.IsNotNull(mappedOrder, "User shouldn't be null when dto is null");

            Assert.AreEqual(mappedOrder.User, orderData.User, "Resulted Order is not the same ");
            Assert.AreEqual(mappedOrder.UserId, orderData.UserId, "Resulted Order is not the same ");
        }

        [TestMethod]
        public void MapToOrderInfos_WhenOrderIsNull_ReturnNull()
        {
            var OrderInfo = OrderMapping.MapToOrderInfos(null);

            Assert.IsNull(OrderInfo, "Order should be null when dto is null");
        }

        [TestMethod]
        public void MapToOrderInfos_WhenOrderIsNotNullButOrderItemsIsNull_ReturnOrderInfoWithNullRecipesIds()
        {
            var OrderInfo = OrderMapping.MapToOrderInfos(order);

            Assert.IsNotNull(OrderInfo, "User shouldn't be null when dto is null");

            Assert.IsNotNull(OrderInfo.User, "Resulted Order Info is not the same ");
            Assert.AreEqual(OrderInfo.Price, 0, "Resulted Order Info is not the same ");
            Assert.IsNotNull(OrderInfo.OrderedMenus, "Resulted Order Info  recipes Id's is not null");
        }

        [TestMethod]
        public void MapToOrderInfos_WhenOrderIsNotNull_ReturnOrderInfo()
        {
            order.OrderItems = new List<OrderItems>() { };
            var OrderInfo = OrderMapping.MapToOrderInfos(order);

            Assert.IsNotNull(OrderInfo, "User shouldn't be null when dto is null");

            Assert.IsNotNull(OrderInfo.User,"Resulted Order Info is not the same ");
            Assert.AreEqual(OrderInfo.Price, 0, "Resulted Order Info is not the same ");
            Assert.IsNotNull(OrderInfo.OrderedMenus, "Resulted Order Info recipes Id's is null");
            Assert.AreEqual(OrderInfo.OrderedMenus.Count, order.OrderItems.Count, "Recipes Id's count should be the same ad Order items count");
        }
    }
}
