using DAWM_Project.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RestaurantAPI.Domain.Dtos.RecipeDtos;
using RestaurantAPI.Domain.Models.MenuRelated;
using RestaurantAPI.Domain.ServicesAbstractions;

namespace RestaurantAPI.Tests.ControllerTests
{
    [TestClass]
    public class RecipeControllerTests : LoggerTests
    {
        private Mock<IRecipeService> _mockRecipeService;
        private RecipesController RecipeController;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRecipeService = new();
            _mockLogger = new();
            RecipeController = new(_mockRecipeService.Object, _mockLogger.Object);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            _mockRecipeService = null;
            _mockLogger = null;
            RecipeController = null;
        }

        [TestMethod]
        public void InstantiatingRecipeController_WhenIUserServiceIsNull_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new RecipesController(null, _mockLogger.Object));
        }

        [TestMethod]
        public void InstantiatingRecipeController_WhenIDataLoggerIsNull_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new RecipesController(_mockRecipeService.Object, null));
        }

        [TestMethod]
        public async Task GetAll__ReturnOkResult()
        {
            _mockRecipeService.Setup(recipeService => recipeService.GetAll());

            var result = await RecipeController.GetAllRecipes();

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
            int recipeId = 1;
            _mockRecipeService.Setup(recipeService => recipeService.GetById(It.IsAny<int>())).ReturnsAsync(() => null);

            var result = await RecipeController.GetRecipeById(recipeId);

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
            int recipeId = 1;
            _mockRecipeService.Setup(recipeService => recipeService.GetById(It.IsAny<int>())).ReturnsAsync(new Recipe());

            var result = await RecipeController.GetRecipeById(recipeId);

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
        public async Task AddRecipe_InputIsOk_ReturnsOkResult()
        {
            CreateOrUpdateRecipe newrecipe = new();

            RecipeController.ControllerContext = new ControllerContext();
            RecipeController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockRecipeService.Setup(x => x.Create(It.IsAny<CreateOrUpdateRecipe>())).ReturnsAsync(true);

            var result = await RecipeController.AddRecipe(newrecipe);

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
        public async Task AddRecipe_InputIsInvalid_ReturnsBadRequestResult()
        {
            CreateOrUpdateRecipe newrecipe = new();

            RecipeController.ControllerContext = new ControllerContext();
            RecipeController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockRecipeService.Setup(x => x.Create(It.IsAny<CreateOrUpdateRecipe>())).ReturnsAsync(false);

            var result = await RecipeController.AddRecipe(newrecipe);

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
        public async Task UpdateRecipe_InputIsOk_ReturnsOkResult()
        {
            CreateOrUpdateRecipe newrecipe = new();
            int recipeid = 1;

            RecipeController.ControllerContext = new ControllerContext();
            RecipeController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockRecipeService.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<CreateOrUpdateRecipe>())).ReturnsAsync(true);

            var result = await RecipeController.UpdateRecipe(recipeid, newrecipe);

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
        public async Task UpdateRecipe_InputIsInvalid_ReturnsBadRequestResult()
        {
            CreateOrUpdateRecipe newrecipe = new();
            int recipeid = 1;

            RecipeController.ControllerContext = new ControllerContext();
            RecipeController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockRecipeService.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<CreateOrUpdateRecipe>())).ReturnsAsync(false);

            var result = await RecipeController.UpdateRecipe(recipeid, newrecipe);

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
        public async Task DeleteRecipe_InputIsOk_ReturnsOkResult()
        {
            int recipeid = 1;

            RecipeController.ControllerContext = new ControllerContext();
            RecipeController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockRecipeService.Setup(x => x.Delete(It.IsAny<int>())).ReturnsAsync(true);

            var result = await RecipeController.DeleteRecipe(recipeid);

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
        public async Task DeleteRecipe_InputIsInvalid_ReturnsBadRequestResult()
        {
            int recipeid = 1;

            RecipeController.ControllerContext = new ControllerContext();
            RecipeController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockRecipeService.Setup(x => x.Delete(It.IsAny<int>())).ReturnsAsync(false);

            var result = await RecipeController.DeleteRecipe(recipeid);

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
