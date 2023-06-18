using DAWM_Project.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RestaurantAPI.Domain.Dtos.MenuDtos;
using RestaurantAPI.Domain.Models.MenuRelated;
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
        public async Task GetAll__ReturnOkResult()
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
        public async Task GetById_WhenInputIsValid_ReturnBadRequestObjectResult()
        {
            int menuId = 1;
            _mockMenuService.Setup(menuService => menuService.GetMenuById(It.IsAny<int>())).ReturnsAsync(() => null);

            var result = await MenuController.GetMenuById(menuId);

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
        public async Task AddMenu_InputIsOk_ReturnsOkResult()
        {
            CreateOrUpdateMenu newmenu = new();

            MenuController.ControllerContext = new ControllerContext();
            MenuController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockMenuService.Setup(x => x.AddMenu(It.IsAny<CreateOrUpdateMenu>())).ReturnsAsync(true);

            var result = await MenuController.AddMenu(newmenu);

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
        public async Task AddMenu_InputIsInvalid_ReturnsBadRequestResult()
        {
            CreateOrUpdateMenu newmenu = new();

            MenuController.ControllerContext = new ControllerContext();
            MenuController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockMenuService.Setup(x => x.AddMenu(It.IsAny<CreateOrUpdateMenu>())).ReturnsAsync(false);

            var result = await MenuController.AddMenu(newmenu);

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
        public async Task UpdateMenu_InputIsOk_ReturnsOkResult()
        {
            CreateOrUpdateMenu newmenu = new();
            int menuid = 1;

            MenuController.ControllerContext = new ControllerContext();
            MenuController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockMenuService.Setup(x => x.UpdateMenu(It.IsAny<int>(), It.IsAny<CreateOrUpdateMenu>())).ReturnsAsync(true);

            var result = await MenuController.UpdateMenu(menuid, newmenu);

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
        public async Task UpdateMenu_InputIsInvalid_ReturnsBadRequestResult()
        {
            CreateOrUpdateMenu newmenu = new();
            int menuid = 1;

            MenuController.ControllerContext = new ControllerContext();
            MenuController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockMenuService.Setup(x => x.UpdateMenu(It.IsAny<int>(), It.IsAny<CreateOrUpdateMenu>())).ReturnsAsync(false);

            var result = await MenuController.UpdateMenu(menuid, newmenu);

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
        public async Task DeleteMenu_InputIsOk_ReturnsOkResult()
        {
            int menuid = 1;

            MenuController.ControllerContext = new ControllerContext();
            MenuController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockMenuService.Setup(x => x.DeleteMenu(It.IsAny<int>())).ReturnsAsync(true);

            var result = await MenuController.DeleteMenu(menuid);

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
        public async Task DeleteMenu_InputIsInvalid_ReturnsBadRequestResult()
        {
            int menuid = 1;

            MenuController.ControllerContext = new ControllerContext();
            MenuController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockMenuService.Setup(x => x.DeleteMenu(It.IsAny<int>())).ReturnsAsync(false);

            var result = await MenuController.DeleteMenu(menuid);

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
        public async Task AddMenuItem_InputIsOk_ReturnsOkResult()
        {
            int menuid = 1;
            int recipeId = 1;

            MenuController.ControllerContext = new ControllerContext();
            MenuController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockMenuService.Setup(x => x.AddMenuItem(It.IsAny<int>(),It.IsAny<int>())).ReturnsAsync(true);

            var result = await MenuController.AddMenuItem(menuid, recipeId);

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
        public async Task AddMenuItem_InputIsInvalid_ReturnsBadRequestResult()
        {
            int menuid = 1;
            int recipeId = 1;

            MenuController.ControllerContext = new ControllerContext();
            MenuController.ControllerContext.HttpContext = new DefaultHttpContext();
            _mockMenuService.Setup(x => x.AddMenuItem(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);

            var result = await MenuController.AddMenuItem(menuid, recipeId);

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
        public async Task DeleteMenuItem_InputIsOk_ReturnsOkResult()
        {
            int menuid = 1;
            int recipeId = 1;

            MenuController.ControllerContext = new ControllerContext();
            MenuController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockMenuService.Setup(x => x.DeleteMenuItem(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

            var result = await MenuController.DeleteMenuItem(menuid, recipeId);

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
        public async Task DeleteMenuItem_InputIsInvalid_ReturnsBadRequestResult()
        {
            int menuid = 1;
            int recipeId = 1;

            MenuController.ControllerContext = new ControllerContext();
            MenuController.ControllerContext.HttpContext = new DefaultHttpContext();
            _mockMenuService.Setup(x => x.DeleteMenuItem(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);

            var result = await MenuController.DeleteMenuItem(menuid, recipeId);

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
