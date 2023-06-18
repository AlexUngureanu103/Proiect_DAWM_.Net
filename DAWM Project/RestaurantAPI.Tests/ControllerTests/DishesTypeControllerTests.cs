using DAWM_Project.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RestaurantAPI.Domain.Dtos.DishesTypeDtos;
using RestaurantAPI.Domain.Models.MenuRelated;
using RestaurantAPI.Domain.ServicesAbstractions;

namespace RestaurantAPI.Tests.ControllerTests
{
    [TestClass]
    public class DishesTypeControllerTests : LoggerTests
    {
        private Mock<IDishesTypeService> _mockDishesTypeService;
        private DishesTypeController dishesTypeController;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockDishesTypeService = new();
            _mockLogger = new();
            dishesTypeController = new(_mockDishesTypeService.Object, _mockLogger.Object);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            _mockDishesTypeService = null;
            _mockLogger = null;
            dishesTypeController = null;
        }

        [TestMethod]
        public void InstantiatingDishesTypeController_WhenIUserServiceIsNull_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new DishesTypeController(null, _mockLogger.Object));
        }

        [TestMethod]
        public void InstantiatingDishesTypeController_WhenIDataLoggerIsNull_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new DishesTypeController(_mockDishesTypeService.Object, null));
        }

        [TestMethod]
        public async Task GetAll__ReturnOkResult()
        {
            _mockDishesTypeService.Setup(dishTypeService => dishTypeService.GetAll());

            var result = await dishesTypeController.GetAllDishesTypes();

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
        public async Task GetById_WhenInputIsValid_ReturnBadRequestObjectResult()
        {
            int dishTypeId = 1;
            _mockDishesTypeService.Setup(dishTypeService => dishTypeService.GetById(It.IsAny<int>())).ReturnsAsync(() => null);

            var result = await dishesTypeController.GetDishesTypeById(dishTypeId);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));

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
            int dishTypeId = 1;
            _mockDishesTypeService.Setup(dishTypeService => dishTypeService.GetById(It.IsAny<int>())).ReturnsAsync(new DishesType());

            var result = await dishesTypeController.GetDishesTypeById(dishTypeId);

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
        public async Task AddDishesType_InputIsOk_ReturnsOkResult()
        {
            CreateOrUpdateDishesType newDishType = new();

            dishesTypeController.ControllerContext = new ControllerContext();
            dishesTypeController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockDishesTypeService.Setup(x => x.Create(It.IsAny<CreateOrUpdateDishesType>())).ReturnsAsync(true);

            var result = await dishesTypeController.AddDishesType(newDishType);

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
        public async Task AddDishesType_InputIsInvalid_ReturnsBadRequestResult()
        {
            CreateOrUpdateDishesType newDishType = new();

            dishesTypeController.ControllerContext = new ControllerContext();
            dishesTypeController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockDishesTypeService.Setup(x => x.Create(It.IsAny<CreateOrUpdateDishesType>())).ReturnsAsync(false);

            var result = await dishesTypeController.AddDishesType(newDishType);

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
        public async Task UpdateDishesType_InputIsOk_ReturnsOkResult()
        {
            CreateOrUpdateDishesType newDishType = new();
            int DishTypeid = 1;

            dishesTypeController.ControllerContext = new ControllerContext();
            dishesTypeController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockDishesTypeService.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<CreateOrUpdateDishesType>())).ReturnsAsync(true);

            var result = await dishesTypeController.UpdateDishesType(DishTypeid, newDishType);

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
        public async Task UpdateDishesType_InputIsInvalid_ReturnsBadRequestResult()
        {
            CreateOrUpdateDishesType newDishType = new();
            int DishTypeid = 1;

            dishesTypeController.ControllerContext = new ControllerContext();
            dishesTypeController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockDishesTypeService.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<CreateOrUpdateDishesType>())).ReturnsAsync(false);

            var result = await dishesTypeController.UpdateDishesType(DishTypeid, newDishType);

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
        public async Task DeleteDishesType_InputIsOk_ReturnsOkResult()
        {
            int DishTypeid = 1;

            dishesTypeController.ControllerContext = new ControllerContext();
            dishesTypeController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockDishesTypeService.Setup(x => x.Delete(It.IsAny<int>())).ReturnsAsync(true);

            var result = await dishesTypeController.DeleteIngredient(DishTypeid);

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
        public async Task DeleteDishesType_InputIsInvalid_ReturnsBadRequestResult()
        {
            int DishTypeid = 1;

            dishesTypeController.ControllerContext = new ControllerContext();
            dishesTypeController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockDishesTypeService.Setup(x => x.Delete(It.IsAny<int>())).ReturnsAsync(false);

            var result = await dishesTypeController.DeleteIngredient(DishTypeid);

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
