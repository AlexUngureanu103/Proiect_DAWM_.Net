using DAWM_Project.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RestaurantAPI.Domain.Dtos.OrderDtos;
using RestaurantAPI.Domain.Dtos.RecipeDtos;
using RestaurantAPI.Domain.ServicesAbstractions;
using System.Security.Claims;

namespace RestaurantAPI.Tests.ControllerTests
{
    [TestClass]
    public class OrderControllerTests : LoggerTests
    {
        private Mock<IOrdersService> _mockOrderService;
        private OrdersController OrderController;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockOrderService = new();
            _mockLogger = new();
            OrderController = new(_mockOrderService.Object, _mockLogger.Object);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            _mockOrderService = null;
            _mockLogger = null;
            OrderController = null;
        }

        [TestMethod]
        public void InstantiatingOrderController_WhenIUserServiceIsNull_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new OrdersController(null, _mockLogger.Object));
        }

        [TestMethod]
        public void InstantiatingOrderController_WhenIDataLoggerIsNull_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new OrdersController(_mockOrderService.Object, null));
        }

        [TestMethod]
        public async Task GetAll__ReturnOkObjectResult()
        {
            OrderController.ControllerContext = new ControllerContext();
            OrderController.ControllerContext.HttpContext = new DefaultHttpContext();
            OrderController.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("userId", 1.ToString())
            }));
            _mockOrderService.Setup(orderService => orderService.GetAll(It.IsAny<int>()));

            var result = await OrderController.GetAllOrders();

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task GetById_WhenInputIsValid_ReturnNotFoundResult()
        {
            int orderId = 1;
            _mockOrderService.Setup(orderService => orderService.GetById(It.IsAny<int>())).ReturnsAsync(() => null);

            var result = await OrderController.GetOrderById(orderId);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task GetById_WhenInputIsInValid_ReturnOkObjectResult()
        {
            int orderId = 1;
            _mockOrderService.Setup(orderService => orderService.GetById(It.IsAny<int>())).ReturnsAsync(new OrderInfo());

            var result = await OrderController.GetOrderById(orderId);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task AddOrder_InputIsOk_ReturnOkResult()
        {
            CreateOrUpdateOrder newOrder = new();

            OrderController.ControllerContext = new ControllerContext();
            OrderController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockOrderService.Setup(orderService => orderService.Create(It.IsAny<CreateOrUpdateOrder>())).ReturnsAsync(true);

            var result = await OrderController.AddOrder(newOrder);

            Assert.IsInstanceOfType(result, typeof(OkResult));

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task AddOrder_InputIsInvalid_ReturnBadRequestResult()
        {
            CreateOrUpdateOrder newOrder = new();

            OrderController.ControllerContext = new ControllerContext();
            OrderController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockOrderService.Setup(orderService => orderService.Create(It.IsAny<CreateOrUpdateOrder>())).ReturnsAsync(false);

            var result = await OrderController.AddOrder(newOrder);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task UpdateOrder_InputIsOk_ReturnOkResult()
        {
            CreateOrUpdateOrder newOrder = new();
            int orderId = 1;

            OrderController.ControllerContext = new ControllerContext();
            OrderController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockOrderService.Setup(orderService => orderService.Update(It.IsAny<int>(), It.IsAny<CreateOrUpdateOrder>())).ReturnsAsync(true);

            var result = await OrderController.UpdateOrder(orderId, newOrder);

            Assert.IsInstanceOfType(result, typeof(OkResult));

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task UpdateOrder_InputIsInvalid_ReturnBadRequestResult()
        {
            CreateOrUpdateOrder newOrder = new();
            int orderId = 1;

            OrderController.ControllerContext = new ControllerContext();
            OrderController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockOrderService.Setup(orderService => orderService.Update(It.IsAny<int>(), It.IsAny<CreateOrUpdateOrder>())).ReturnsAsync(false);

            var result = await OrderController.UpdateOrder(orderId, newOrder);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task DeleteOrder_InputIsOk_ReturnOkResult()
        {
            int orderId = 1;

            OrderController.ControllerContext = new ControllerContext();
            OrderController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockOrderService.Setup(orderService => orderService.Delete(It.IsAny<int>())).ReturnsAsync(true);

            var result = await OrderController.DeleteOrder(orderId);

            Assert.IsInstanceOfType(result, typeof(OkResult));

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task DeleteOrder_InputIsInvalid_ReturnBadRequestResult()
        {
            int orderId = 1;

            OrderController.ControllerContext = new ControllerContext();
            OrderController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockOrderService.Setup(orderService => orderService.Delete(It.IsAny<int>())).ReturnsAsync(false);

            var result = await OrderController.DeleteOrder(orderId);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task AddOrderItem_InputIsOk_ReturnOkResult()
        {
            int orderId = 1;
            int recipeId = 1;

            OrderController.ControllerContext = new ControllerContext();
            OrderController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockOrderService.Setup(orderService => orderService.AddOrderItem(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

            var result = await OrderController.AddOrderItem(orderId, recipeId);

            Assert.IsInstanceOfType(result, typeof(OkResult));

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task AddOrderItem_InputIsInvalid_ReturnBadRequestResult()
        {
            int orderId = 1;
            int recipeId = 1;

            OrderController.ControllerContext = new ControllerContext();
            OrderController.ControllerContext.HttpContext = new DefaultHttpContext();
            _mockOrderService.Setup(orderService => orderService.AddOrderItem(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);

            var result = await OrderController.AddOrderItem(orderId, recipeId);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task DeleteOrderItem_InputIsOk_ReturnOkResult()
        {
            int orderId = 1;
            int recipeId = 1;

            OrderController.ControllerContext = new ControllerContext();
            OrderController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockOrderService.Setup(orderService => orderService.DeleteOrderItem(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

            var result = await OrderController.DeleteOrderItem(orderId, recipeId);

            Assert.IsInstanceOfType(result, typeof(OkResult));

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task DeleteOrderItem_InputIsInvalid_ReturnBadRequestResult()
        {
            int orderId = 1;
            int recipeId = 1;

            OrderController.ControllerContext = new ControllerContext();
            OrderController.ControllerContext.HttpContext = new DefaultHttpContext();
            _mockOrderService.Setup(orderService => orderService.DeleteOrderItem(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);

            var result = await OrderController.DeleteOrderItem(orderId, recipeId);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task AddOrderSingleItem_InputIsOk_ReturnOkResult()
        {
            int orderId = 1;
            int recipeId = 1;

            OrderController.ControllerContext = new ControllerContext();
            OrderController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockOrderService.Setup(orderService => orderService.AddOrderSingleItem(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

            var result = await OrderController.AddOrderSingleItem(orderId, recipeId);

            Assert.IsInstanceOfType(result, typeof(OkResult));

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task AddOrderSingleItem_InputIsInvalid_ReturnBadRequestResult()
        {
            int orderId = 1;
            int recipeId = 1;

            OrderController.ControllerContext = new ControllerContext();
            OrderController.ControllerContext.HttpContext = new DefaultHttpContext();
            _mockOrderService.Setup(orderService => orderService.AddOrderSingleItem(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);

            var result = await OrderController.AddOrderSingleItem(orderId, recipeId);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task DeleteOrderSingleItem_InputIsOk_ReturnOkResult()
        {
            int orderId = 1;
            int recipeId = 1;

            OrderController.ControllerContext = new ControllerContext();
            OrderController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockOrderService.Setup(orderService => orderService.DeleteOrderSingleItem(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

            var result = await OrderController.DeleteOrderSingleItem(orderId, recipeId);

            Assert.IsInstanceOfType(result, typeof(OkResult));

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task DeleteOrderSingleItem_InputIsInvalid_ReturnBadRequestResult()
        {
            int orderId = 1;
            int recipeId = 1;

            OrderController.ControllerContext = new ControllerContext();
            OrderController.ControllerContext.HttpContext = new DefaultHttpContext();
            _mockOrderService.Setup(orderService => orderService.DeleteOrderSingleItem(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);

            var result = await OrderController.DeleteOrderSingleItem(orderId, recipeId);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }
    }
}
