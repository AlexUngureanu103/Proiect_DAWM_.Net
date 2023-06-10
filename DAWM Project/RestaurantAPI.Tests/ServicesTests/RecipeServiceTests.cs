using Core.Services;
using Moq;
using RestaurantAPI.Domain;
using RestaurantAPI.Domain.Dtos.RecipeDtos;
using RestaurantAPI.Domain.Models.MenuRelated;
using RestaurantAPI.Exceptions;

namespace RestaurantAPI.Tests.ServicesTests
{
    [TestClass]
    public class RecipeServiceTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IDataLogger> _mockLogger;
        private CreateOrUpdateRecipe recipeData;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockUnitOfWork = new();
            _mockLogger = new();
            recipeData = new()
            {
                Name = "test",
                DishesTypeId = 1,
                Price = 12
            };
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            _mockUnitOfWork = null;
            _mockLogger = null;
            recipeData = null;
        }

        [TestMethod]
        public void InstantiatingRecipeService_WhenUnitOfWorkIsNull_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new RecipeService(null, _mockLogger.Object));
        }

        [TestMethod]
        public void InstantiatingRecipeService_WhenIDataLoggerIsNull_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new RecipeService(_mockUnitOfWork.Object, null));
        }

        [TestMethod]
        public async Task GetAllRecipes_WhenCalled_ReturnsAllRecipes()
        {
            List<Recipe> recipes = new()
            {
                new Recipe
                {
                    Id = 1,
                    Name = "Recipe 1",
                    Price= 12,
                    DishesTypeId=1,
                },
                new Recipe
                {
                    Id = 2,
                    Name = "Recipe 2",
                    Price= 12,
                    DishesTypeId=2,
                }
            };

            _mockUnitOfWork.Setup(x => x.RecipeRepository.GetAllAsync()).ReturnsAsync(recipes);

            var RecipeService = new RecipeService(_mockUnitOfWork.Object, _mockLogger.Object);

            var result = await RecipeService.GetAll();

            Assert.IsNotNull(result, "Recipe list shouldn't be null");
            Assert.AreEqual(2, result.Count(), "Recipe list should only contain 2 elements");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task GetRecipeById_WhenRecipeIsFound_ReturnRecipe()
        {
            List<Recipe> recipes = new()
            {
                new Recipe
                {
                    Id = 1,
                    Name = "Recipe 1",
                    Price= 12,
                    DishesTypeId=1,
                },
                new Recipe
                {
                    Id = 2,
                    Name = "Recipe 2",
                    Price= 12,
                    DishesTypeId=2,
                }
            };
            int id = 1;

            _mockUnitOfWork.Setup(x => x.RecipeRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(recipes[1]);

            var RecipeService = new RecipeService(_mockUnitOfWork.Object, _mockLogger.Object);

            var result = await RecipeService.GetById(id);

            Assert.IsNotNull(result, "Recipe should be found");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task GetRecipeById_WhenRecipeIsNotFound_ReturnNull()
        {
            List<Recipe> recipes = new()
            {
                new Recipe
                {
                    Id = 1,
                    Name = "Recipe 1",
                    Price= 12,
                    DishesTypeId=1,
                },
                new Recipe
                {
                    Id = 2,
                    Name = "Recipe 2",
                    Price= 12,
                    DishesTypeId=2,
                }
            };
            int id = 351;

            _mockUnitOfWork.Setup(x => x.RecipeRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return null;
            });

            var RecipeService = new RecipeService(_mockUnitOfWork.Object, _mockLogger.Object);

            var result = await RecipeService.GetById(id);

            Assert.IsNull(result, "Recipe shouldn't be found");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task UpdateRecipe_WhenRecipeIsNull_ThrowArguemntNullException()
        {
            var RecipeService = new RecipeService(_mockUnitOfWork.Object, _mockLogger.Object);

            Assert.ThrowsExceptionAsync<ArgumentNullException>(() => RecipeService.Update(1, null));

            TestLoggerMethods(
                logErrorCount: 1,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task UpdateRecipe_WhenRecipeIsOk_ReturnTrue()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.SaveChangesAsync()).ReturnsAsync(true);
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.RecipeRepository.UpdateAsync(It.IsAny<int>(), It.IsAny<Recipe>()));

            var RecipeService = new RecipeService(_mockUnitOfWork.Object, _mockLogger.Object);

            bool result = await RecipeService.Update(1, recipeData);

            Assert.IsTrue(result, "Recipe should be updates");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task DeleteRecipe_WhenRecipeIsNotOk_ReturnFalse()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.RecipeRepository.DeleteAsync(It.IsAny<int>())).ThrowsAsync(new EntityNotFoundException());

            var RecipeService = new RecipeService(_mockUnitOfWork.Object, _mockLogger.Object);

            bool result = await RecipeService.Delete(1);

            Assert.IsTrue(!result, "Recipe deletion should fail");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 1,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task DeleteRecipe_WhenRecipeIsOk_ReturnTrue()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.RecipeRepository.DeleteAsync(It.IsAny<int>()));
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.SaveChangesAsync()).ReturnsAsync(true);

            var RecipeService = new RecipeService(_mockUnitOfWork.Object, _mockLogger.Object);

            bool result = await RecipeService.Delete(1);

            Assert.IsTrue(result, "Recipe deletion should fail");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task AddRecipe_WhenRecipeIsOk_ReturnTrue()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.RecipeRepository.AddAsync(It.IsAny<Recipe>()));
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.SaveChangesAsync()).ReturnsAsync(true);

            var RecipeService = new RecipeService(_mockUnitOfWork.Object, _mockLogger.Object);

            bool result = await RecipeService.Create(recipeData);

            Assert.IsTrue(result, "Recipe creation shouldn't fail");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task AddRecipe_WhenRecipeIsNull_ThrowArgumentNullException()
        {
            var RecipeService = new RecipeService(_mockUnitOfWork.Object, _mockLogger.Object);

            Assert.ThrowsExceptionAsync<ArgumentNullException>(() => RecipeService.Create(null));

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        /// <summary>
        /// Tests the how many times the logger methods have been used
        /// </summary>
        /// <param name="logErrorCount">LogError counter</param>
        /// <param name="logErrorExCount">LogError with Exception counter</param>
        /// <param name="logWarnCount">LogWarn counter</param>
        /// <param name="logInfoCount">LogInfo counter</param>
        /// <param name="logDebugCount">LogDebug counter</param>
        private void TestLoggerMethods(int logErrorCount, int logErrorExCount, int logWarnCount, int logInfoCount, int logDebugCount)
        {
            _mockLogger.Verify(log => log.LogError(It.IsAny<string>()), Times.Exactly(logErrorCount));
            _mockLogger.Verify(log => log.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Exactly(logErrorExCount));
            _mockLogger.Verify(log => log.LogWarn(It.IsAny<string>()), Times.Exactly(logWarnCount));
            _mockLogger.Verify(log => log.LogInfo(It.IsAny<string>()), Times.Exactly(logInfoCount));
            _mockLogger.Verify(log => log.LogDebug(It.IsAny<string>()), Times.Exactly(logDebugCount));
        }
    }
}
