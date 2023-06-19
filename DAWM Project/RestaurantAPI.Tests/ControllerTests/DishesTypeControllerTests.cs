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
        public async Task GetAll__ReturnOkObjectResult()
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
        public async Task GetById_WhenInputIsValid_ReturnNotFoundResult()
        {
            int dishTypeId = 1;
            _mockDishesTypeService.Setup(dishTypeService => dishTypeService.GetById(It.IsAny<int>())).ReturnsAsync(() => null);

            var result = await dishesTypeController.GetDishesTypeById(dishTypeId);

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
        public async Task AddDishesType_InputIsOk_ReturnOkResult()
        {
            CreateOrUpdateDishesType newDishType = new();

            dishesTypeController.ControllerContext = new ControllerContext();
            dishesTypeController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockDishesTypeService.Setup(dishesTypeService => dishesTypeService.Create(It.IsAny<CreateOrUpdateDishesType>())).ReturnsAsync(true);

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
        public async Task AddDishesType_InputIsInvalid_ReturnBadRequestResult()
        {
            CreateOrUpdateDishesType newDishType = new();

            dishesTypeController.ControllerContext = new ControllerContext();
            dishesTypeController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockDishesTypeService.Setup(dishesTypeService => dishesTypeService.Create(It.IsAny<CreateOrUpdateDishesType>())).ReturnsAsync(false);

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
        public async Task UpdateDishesType_InputIsOk_ReturnOkResult()
        {
            CreateOrUpdateDishesType newDishType = new();
            int dishTypeid = 1;

            dishesTypeController.ControllerContext = new ControllerContext();
            dishesTypeController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockDishesTypeService.Setup(dishesTypeService => dishesTypeService.Update(It.IsAny<int>(), It.IsAny<CreateOrUpdateDishesType>())).ReturnsAsync(true);

            var result = await dishesTypeController.UpdateDishesType(dishTypeid, newDishType);

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
        public async Task UpdateDishesType_InputIsInvalid_ReturnBadRequestResult()
        {
            CreateOrUpdateDishesType newDishType = new();
            int dishTypeid = 1;

            dishesTypeController.ControllerContext = new ControllerContext();
            dishesTypeController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockDishesTypeService.Setup(dishesTypeService => dishesTypeService.Update(It.IsAny<int>(), It.IsAny<CreateOrUpdateDishesType>())).ReturnsAsync(false);

            var result = await dishesTypeController.UpdateDishesType(dishTypeid, newDishType);

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
        public async Task DeleteDishesType_InputIsOk_ReturnOkResult()
        {
            int dishTypeid = 1;

            dishesTypeController.ControllerContext = new ControllerContext();
            dishesTypeController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockDishesTypeService.Setup(dishesTypeService => dishesTypeService.Delete(It.IsAny<int>())).ReturnsAsync(true);

            var result = await dishesTypeController.DeleteIngredient(dishTypeid);

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
        public async Task DeleteDishesType_InputIsInvalid_ReturnBadRequestResult()
        {
            int dishTypeid = 1;

            dishesTypeController.ControllerContext = new ControllerContext();
            dishesTypeController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockDishesTypeService.Setup(dishesTypeService => dishesTypeService.Delete(It.IsAny<int>())).ReturnsAsync(false);

            var result = await dishesTypeController.DeleteIngredient(dishTypeid);

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
