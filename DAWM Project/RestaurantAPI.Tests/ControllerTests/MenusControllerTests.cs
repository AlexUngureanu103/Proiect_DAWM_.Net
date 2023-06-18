using DAWM_Project.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RestaurantAPI.Domain.Dtos.MenuDtos;
using RestaurantAPI.Domain.ServicesAbstractions;

namespace RestaurantAPI.Tests.ControllerTests
{
    [TestClass]
    public class MenuControllerTests : LoggerTests
    {
        private Mock<IMenusService> _mockMenuService;
        private MenusController MenuController;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockMenuService = new();
            _mockLogger = new();
            MenuController = new(_mockMenuService.Object, _mockLogger.Object);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            _mockMenuService = null;
            _mockLogger = null;
            MenuController = null;
        }

        [TestMethod]
        public void InstantiatingMenuController_WhenIUserServiceIsNull_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new MenusController(null, _mockLogger.Object));
        }

        [TestMethod]
        public void InstantiatingMenuController_WhenIDataLoggerIsNull_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new MenusController(_mockMenuService.Object, null));
        }

        [TestMethod]
        public async Task GetAll__ReturnOkObjectResult()
        {
            _mockMenuService.Setup(menuService => menuService.GetAllMenus());

            var result = await MenuController.GetAllMenus();

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
            int menuId = 1;
            _mockMenuService.Setup(menuService => menuService.GetMenuById(It.IsAny<int>())).ReturnsAsync(() => null);

            var result = await MenuController.GetMenuById(menuId);

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
            int menuId = 1;
            _mockMenuService.Setup(menuService => menuService.GetMenuById(It.IsAny<int>())).ReturnsAsync(new MenuInfos());

            var result = await MenuController.GetMenuById(menuId);

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
        public async Task AddMenu_InputIsOk_ReturnOkResult()
        {
            CreateOrUpdateMenu newMenu = new();

            MenuController.ControllerContext = new ControllerContext();
            MenuController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockMenuService.Setup(menuService => menuService.AddMenu(It.IsAny<CreateOrUpdateMenu>())).ReturnsAsync(true);

            var result = await MenuController.AddMenu(newMenu);

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
        public async Task AddMenu_InputIsInvalid_ReturnBadRequestResult()
        {
            CreateOrUpdateMenu newMenu = new();

            MenuController.ControllerContext = new ControllerContext();
            MenuController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockMenuService.Setup(menuService => menuService.AddMenu(It.IsAny<CreateOrUpdateMenu>())).ReturnsAsync(false);

            var result = await MenuController.AddMenu(newMenu);

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
        public async Task UpdateMenu_InputIsOk_ReturnOkResult()
        {
            CreateOrUpdateMenu newMenu = new();
            int menuId = 1;

            MenuController.ControllerContext = new ControllerContext();
            MenuController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockMenuService.Setup(menuService => menuService.UpdateMenu(It.IsAny<int>(), It.IsAny<CreateOrUpdateMenu>())).ReturnsAsync(true);

            var result = await MenuController.UpdateMenu(menuId, newMenu);

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
        public async Task UpdateMenu_InputIsInvalid_ReturnBadRequestResult()
        {
            CreateOrUpdateMenu newMenu = new();
            int menuId = 1;

            MenuController.ControllerContext = new ControllerContext();
            MenuController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockMenuService.Setup(menuService => menuService.UpdateMenu(It.IsAny<int>(), It.IsAny<CreateOrUpdateMenu>())).ReturnsAsync(false);

            var result = await MenuController.UpdateMenu(menuId, newMenu);

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
        public async Task DeleteMenu_InputIsOk_ReturnOkResult()
        {
            int menuId = 1;

            MenuController.ControllerContext = new ControllerContext();
            MenuController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockMenuService.Setup(menuService => menuService.DeleteMenu(It.IsAny<int>())).ReturnsAsync(true);

            var result = await MenuController.DeleteMenu(menuId);

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
        public async Task DeleteMenu_InputIsInvalid_ReturnBadRequestResult()
        {
            int menuId = 1;

            MenuController.ControllerContext = new ControllerContext();
            MenuController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockMenuService.Setup(menuService => menuService.DeleteMenu(It.IsAny<int>())).ReturnsAsync(false);

            var result = await MenuController.DeleteMenu(menuId);

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
        public async Task AddMenuItem_InputIsOk_ReturnOkResult()
        {
            int menuId = 1;
            int recipeId = 1;

            MenuController.ControllerContext = new ControllerContext();
            MenuController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockMenuService.Setup(menuService => menuService.AddMenuItem(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

            var result = await MenuController.AddMenuItem(menuId, recipeId);

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
        public async Task AddMenuItem_InputIsInvalid_ReturnBadRequestResult()
        {
            int menuId = 1;
            int recipeId = 1;

            MenuController.ControllerContext = new ControllerContext();
            MenuController.ControllerContext.HttpContext = new DefaultHttpContext();
            _mockMenuService.Setup(menuService => menuService.AddMenuItem(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);

            var result = await MenuController.AddMenuItem(menuId, recipeId);

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
        public async Task DeleteMenuItem_InputIsOk_ReturnOkResult()
        {
            int menuId = 1;
            int recipeId = 1;

            MenuController.ControllerContext = new ControllerContext();
            MenuController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockMenuService.Setup(menuService => menuService.DeleteMenuItem(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

            var result = await MenuController.DeleteMenuItem(menuId, recipeId);

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
        public async Task DeleteMenuItem_InputIsInvalid_ReturnBadRequestResult()
        {
            int menuId = 1;
            int recipeId = 1;

            MenuController.ControllerContext = new ControllerContext();
            MenuController.ControllerContext.HttpContext = new DefaultHttpContext();
            _mockMenuService.Setup(menuService => menuService.DeleteMenuItem(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);

            var result = await MenuController.DeleteMenuItem(menuId, recipeId);

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
