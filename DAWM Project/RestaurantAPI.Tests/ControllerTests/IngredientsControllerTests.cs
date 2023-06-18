using DAWM_Project.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RestaurantAPI.Domain.Dtos.IngredientDtos;
using RestaurantAPI.Domain.Models.MenuRelated;
using RestaurantAPI.Domain.ServicesAbstractions;

namespace RestaurantAPI.Tests.ControllerTests
{
    [TestClass]
    public class IngredientControllerTests : LoggerTests
    {
        private Mock<IIngredientsService> _mockIngredientService;
        private IngredientsController IngredientController;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockIngredientService = new();
            _mockLogger = new();
            IngredientController = new(_mockIngredientService.Object, _mockLogger.Object);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            _mockIngredientService = null;
            _mockLogger = null;
            IngredientController = null;
        }

        [TestMethod]
        public void InstantiatingIngredientController_WhenIUserServiceIsNull_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new IngredientsController(null, _mockLogger.Object));
        }

        [TestMethod]
        public void InstantiatingIngredientController_WhenIDataLoggerIsNull_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new IngredientsController(_mockIngredientService.Object, null));
        }

        [TestMethod]
        public async Task GetAll__ReturnOkResult()
        {
            _mockIngredientService.Setup(ingredientService => ingredientService.GetAll());

            var result = await IngredientController.GetAllIngredients();

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
            int ingredientId = 1;
            _mockIngredientService.Setup(ingredientService => ingredientService.GetById(It.IsAny<int>())).ReturnsAsync(() => null);

            var result = await IngredientController.GetIngredientById(ingredientId);

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
            int ingredientId = 1;
            _mockIngredientService.Setup(ingredientService => ingredientService.GetById(It.IsAny<int>())).ReturnsAsync(new Ingredient());

            var result = await IngredientController.GetIngredientById(ingredientId);

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
        public async Task AddIngredient_InputIsOk_ReturnsOkResult()
        {
            CreateOrUpdateIngredient newingredient = new();

            IngredientController.ControllerContext = new ControllerContext();
            IngredientController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockIngredientService.Setup(x => x.Create(It.IsAny<CreateOrUpdateIngredient>())).ReturnsAsync(true);

            var result = await IngredientController.AddIngredient(newingredient);

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
        public async Task AddIngredient_InputIsInvalid_ReturnsBadRequestResult()
        {
            CreateOrUpdateIngredient newingredient = new();

            IngredientController.ControllerContext = new ControllerContext();
            IngredientController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockIngredientService.Setup(x => x.Create(It.IsAny<CreateOrUpdateIngredient>())).ReturnsAsync(false);

            var result = await IngredientController.AddIngredient(newingredient);

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
        public async Task UpdateIngredient_InputIsOk_ReturnsOkResult()
        {
            CreateOrUpdateIngredient newingredient = new();
            int ingredientid = 1;

            IngredientController.ControllerContext = new ControllerContext();
            IngredientController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockIngredientService.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<CreateOrUpdateIngredient>())).ReturnsAsync(true);

            var result = await IngredientController.UpdateIngredient(ingredientid, newingredient);

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
        public async Task UpdateIngredient_InputIsInvalid_ReturnsBadRequestResult()
        {
            CreateOrUpdateIngredient newingredient = new();
            int ingredientid = 1;

            IngredientController.ControllerContext = new ControllerContext();
            IngredientController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockIngredientService.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<CreateOrUpdateIngredient>())).ReturnsAsync(false);

            var result = await IngredientController.UpdateIngredient(ingredientid, newingredient);

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
        public async Task DeleteIngredient_InputIsOk_ReturnsOkResult()
        {
            int ingredientid = 1;

            IngredientController.ControllerContext = new ControllerContext();
            IngredientController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockIngredientService.Setup(x => x.Delete(It.IsAny<int>())).ReturnsAsync(true);

            var result = await IngredientController.DeleteIngredient(ingredientid);

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
        public async Task DeleteIngredient_InputIsInvalid_ReturnsBadRequestResult()
        {
            int ingredientid = 1;

            IngredientController.ControllerContext = new ControllerContext();
            IngredientController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockIngredientService.Setup(x => x.Delete(It.IsAny<int>())).ReturnsAsync(false);

            var result = await IngredientController.DeleteIngredient(ingredientid);

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
