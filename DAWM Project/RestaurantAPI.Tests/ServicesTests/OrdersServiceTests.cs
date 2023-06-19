using Core.Services;
using Moq;
using RestaurantAPI.Domain;
using RestaurantAPI.Domain.Dtos.OrderDtos;
using RestaurantAPI.Domain.Models.MenuRelated;
using RestaurantAPI.Domain.Models.Orders;
using RestaurantAPI.Domain.Models.Users;
using RestaurantAPI.Exceptions;

namespace RestaurantAPI.Tests.ServicesTests
{
    [TestClass]
    public class OrdersServiceTests : LoggerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private CreateOrUpdateOrder orderData;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockUnitOfWork = new();
            _mockLogger = new();
            orderData = new()
            {
                UserId = 1,
                User = new User { Id = 1 }
            };
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            _mockUnitOfWork = null;
            _mockLogger = null;
            orderData = null;
        }

        [TestMethod]
        public void InstantiatingOrdersService_WhenUnitOfWorkIsNull_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new OrdersService(null, _mockLogger.Object));
        }

        [TestMethod]
        public void InstantiatingOrdersService_WhenIDataLoggerIsNull_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new OrdersService(_mockUnitOfWork.Object, null));
        }

        [TestMethod]
        public async Task GetAllOrders_WhenCalled_ReturnsAllOrders()
        {
            List<Order> Orders = new()
            {
                new Order
                {
                    Id = 1,
                    OrderDate = DateTime.Now,
                    OrderItems = new(),
                    OrderSingleItems = new(),
                    UserId = 1,
                    User = new User { Id = 1 }
                },
                new Order
                {
                    Id = 2,
                    OrderDate = DateTime.Now,
                    OrderItems = new(),
                    OrderSingleItems = new(),
                    UserId = 1,
                    User = new User { Id = 1 }
                },
                new Order
                {
                    Id = 3,
                    OrderDate = DateTime.Now,
                    OrderItems = new(),
                    OrderSingleItems = new(),
                    UserId = 2,
                    User = new User { Id = 2 }
                }
            };

            _mockUnitOfWork.Setup(x => x.OrdersRepository.GetAllAsync()).ReturnsAsync(Orders);

            var OrdersService = new OrdersService(_mockUnitOfWork.Object, _mockLogger.Object);

            var result = await OrdersService.GetAll();

            Assert.IsNotNull(result, "Order list shouldn't be null");
            Assert.AreEqual(3, result.Count(), "Order list should only contain 2 elements");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task GetOrderById_WhenOrderIsFound_ReturnOrder()
        {
            List<Order> Orders = new()
            {
                new Order
                {
                    Id = 1,
                    OrderDate = DateTime.Now,
                    OrderItems = new(),
                    OrderSingleItems = new(),
                    UserId = 1,
                    User = new User { Id = 1 }
                },
                new Order
                {
                    Id = 2,
                    OrderDate = DateTime.Now,
                    OrderItems = new(),
                    OrderSingleItems = new(),
                    UserId = 1,
                    User = new User { Id = 1 }
                },
                new Order
                {
                    Id = 3,
                    OrderDate = DateTime.Now,
                    OrderItems = new(),
                    OrderSingleItems = new(),
                    UserId = 2,
                    User = new User { Id = 2 }
                }
            };
            int id = 1;

            _mockUnitOfWork.Setup(x => x.OrdersRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(Orders[1]);

            var OrdersService = new OrdersService(_mockUnitOfWork.Object, _mockLogger.Object);

            var result = await OrdersService.GetById(id);

            Assert.IsNotNull(result, "Order should be found");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task GetOrderById_WhenOrderIsNotFound_ReturnNull()
        {
            List<Order> Orders = new()
            {
                new Order
                {
                    Id = 1,
                    OrderDate = DateTime.Now,
                    OrderItems = new(),
                    OrderSingleItems = new(),
                    UserId = 1,
                    User = new User { Id = 1 }
                },
                new Order
                {
                    Id = 2,
                    OrderDate = DateTime.Now,
                    OrderItems = new(),
                    OrderSingleItems = new(),
                    UserId = 1,
                    User = new User { Id = 1 }
                },
                new Order
                {
                    Id = 3,
                    OrderDate = DateTime.Now,
                    OrderItems = new(),
                    OrderSingleItems = new(),
                    UserId = 2,
                    User = new User { Id = 2 }
                }
            };
            int id = 351;

            _mockUnitOfWork.Setup(x => x.OrdersRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return null;
            });

            var OrdersService = new OrdersService(_mockUnitOfWork.Object, _mockLogger.Object);

            var result = await OrdersService.GetById(id);

            Assert.IsNull(result, "Order shouldn't be found");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task UpdateOrder_WhenOrderIsNull_ThrowArguemntNullException()
        {
            var OrdersService = new OrdersService(_mockUnitOfWork.Object, _mockLogger.Object);

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => OrdersService.Update(1, null));

            TestLoggerMethods(
                logErrorCount: 1,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task UpdateOrder_WhenOrderIsOk_ReturnTrue()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.SaveChangesAsync()).ReturnsAsync(true);
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.OrdersRepository.UpdateAsync(It.IsAny<int>(), It.IsAny<Order>()));

            var OrdersService = new OrdersService(_mockUnitOfWork.Object, _mockLogger.Object);

            bool result = await OrdersService.Update(1, orderData);

            Assert.IsTrue(result, "Order should  Update successfully");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 1,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task DeleteOrder_WhenOrderIsNotOk_ReturnFalse()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.OrdersRepository.DeleteAsync(It.IsAny<int>())).ThrowsAsync(new EntityNotFoundException());

            var OrdersService = new OrdersService(_mockUnitOfWork.Object, _mockLogger.Object);

            bool result = await OrdersService.Delete(1);

            Assert.IsTrue(!result, "Order deletion should fail");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 1,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task DeleteOrder_WhenOrderIsOk_ReturnTrue()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.OrdersRepository.DeleteAsync(It.IsAny<int>()));
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.SaveChangesAsync()).ReturnsAsync(true);

            var OrdersService = new OrdersService(_mockUnitOfWork.Object, _mockLogger.Object);

            bool result = await OrdersService.Delete(1);

            Assert.IsTrue(result, "Order deletion should not fail");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 1,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task AddOrder_WhenOrderIsOk_ReturnTrue()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.OrdersRepository.AddAsync(It.IsAny<Order>()));
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.SaveChangesAsync()).ReturnsAsync(true);

            var OrdersService = new OrdersService(_mockUnitOfWork.Object, _mockLogger.Object);

            bool result = await OrdersService.Create(orderData);

            Assert.IsTrue(result, "Order creation shouldn't fail");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 1,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task AddOrder_WhenOrderIsNull_ThrowArgumentNullException()
        {
            var OrdersService = new OrdersService(_mockUnitOfWork.Object, _mockLogger.Object);

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => OrdersService.Create(null));

            TestLoggerMethods(
                logErrorCount: 1,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task AddOrderItem_WhenOrderIsNotFound_ReturnFalse()
        {
            _mockUnitOfWork.Setup(x => x.OrdersRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return null;
            });

            var OrdersService = new OrdersService(_mockUnitOfWork.Object, _mockLogger.Object);

            bool result = await OrdersService.AddOrderItem(0, 1);

            Assert.IsFalse(result, "Order shouldn't be found");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 1,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task AddOrderItem_WhenMenuIsNotFound_ReturnFalse()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.OrdersRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return new Order();
            });
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.MenusRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return null;
            });

            var OrdersService = new OrdersService(_mockUnitOfWork.Object, _mockLogger.Object);

            bool result = await OrdersService.AddOrderItem(0, 1);

            Assert.IsFalse(result, "Menu shouldn't be found");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 1,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task AddOrderItem_WhenEverythinIsOk_ReturnTrue()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.OrdersRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return new Order { OrderItems = new() };
            });
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.MenusRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return new Menu { Id = 1 };
            });
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.SaveChangesAsync()).ReturnsAsync(() => { return true; });

            var OrdersService = new OrdersService(_mockUnitOfWork.Object, _mockLogger.Object);

            bool result = await OrdersService.AddOrderItem(0, 1);

            Assert.IsTrue(result, "Order should update successfully");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 1,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task DeleteOrderItem_WhenOrderIsNotFound_ReturnFalse()
        {
            _mockUnitOfWork.Setup(x => x.OrdersRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return null;
            });

            var OrdersService = new OrdersService(_mockUnitOfWork.Object, _mockLogger.Object);

            bool result = await OrdersService.DeleteOrderItem(0, 1);

            Assert.IsFalse(result, "Order shouldn't be found");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 1,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task DeleteOrderItem_WhenMenuIsNotFound_ReturnFalse()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.OrdersRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return new Order { OrderItems = new() };
            });

            var OrdersService = new OrdersService(_mockUnitOfWork.Object, _mockLogger.Object);

            bool result = await OrdersService.DeleteOrderItem(0, 1);

            Assert.IsFalse(result, "Menu shouldn't be found");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 1,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task DeleteOrderItem_WhenEverythingIsOk_ReturnTrue()
        {
            int orderItemId = 1;
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.OrdersRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return new Order { OrderItems = new() { new OrderItems { MenuId = orderItemId } } };
            });
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.SaveChangesAsync()).ReturnsAsync(() => { return true; });

            var OrdersService = new OrdersService(_mockUnitOfWork.Object, _mockLogger.Object);

            bool result = await OrdersService.DeleteOrderItem(0, orderItemId);

            Assert.IsTrue(result, "Menu shouldn't be found");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 1,
                logDebugCount: 0
                );
        }
    }
}
