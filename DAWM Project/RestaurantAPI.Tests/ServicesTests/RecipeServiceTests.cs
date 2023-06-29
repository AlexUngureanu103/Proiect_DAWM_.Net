using Core.Services;
using Moq;
using RestaurantAPI.Domain;
using RestaurantAPI.Domain.Dtos.RecipeDtos;
using RestaurantAPI.Domain.Models.MenuRelated;
using RestaurantAPI.Exceptions;

namespace RestaurantAPI.Tests.ServicesTests
{
    [TestClass]
    public class RecipeServiceTests : LoggerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private CreateOrUpdateRecipe recipeData;
        List<Recipe> recipes;

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
            recipes = new()
            {
                new Recipe
                {
                    Id = 1,
                    Name = "Recipe 1",
                    Price= 12,
                    DishesTypeId=1,
                    Ingredients = new List<RecipeIngredient>
                    {
                        new RecipeIngredient
                        {
                            IngredientId = 1,
                            RecipeId = 1
                        }
                    }
                },
                new Recipe
                {
                    Id = 2,
                    Name = "Recipe 2",
                    Price= 12,
                    DishesTypeId=2,
                    Ingredients = new List<RecipeIngredient>
                    {
                        new RecipeIngredient
                        {
                            IngredientId = 2,
                            RecipeId = 2
                        }
                    }
                }
            };
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            _mockUnitOfWork = null;
            _mockLogger = null;
            recipeData = null;
            recipes = null;
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
            _mockUnitOfWork.Setup(x => x.RecipeRepository.GetAllWithIngredients()).ReturnsAsync(recipes);

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

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => RecipeService.Update(1, null));

            TestLoggerMethods(
                logErrorCount: 1,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task UpdateRecipe_WhenDishTypeIsNotFound_ReturnFalse()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.SaveChangesAsync()).ReturnsAsync(true);
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.RecipeRepository.UpdateAsync(It.IsAny<int>(), It.IsAny<Recipe>()));
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.DishesTypeRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(() => null);

            var RecipeService = new RecipeService(_mockUnitOfWork.Object, _mockLogger.Object);

            bool result = await RecipeService.Update(1, recipeData);

            Assert.IsTrue(!result, "Recipe shouldn't update successfully");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 1,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task UpdateRecipe_WhenRecipeIsNotFound_ReturnFalse()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.SaveChangesAsync()).ReturnsAsync(true);
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.RecipeRepository.UpdateAsync(It.IsAny<int>(), It.IsAny<Recipe>())).ThrowsAsync(new EntityNotFoundException("Recipe not found"));
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.DishesTypeRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new DishesType());

            var RecipeService = new RecipeService(_mockUnitOfWork.Object, _mockLogger.Object);

            bool result = await RecipeService.Update(1, recipeData);

            Assert.IsTrue(!result, "Recipe shouldn't update successfully");

            TestLoggerMethods(
               logErrorCount: 0,
               logErrorExCount: 1,
               logWarnCount: 0,
               logInfoCount: 0,
               logDebugCount: 0
               );
        }

        [TestMethod]
        public async Task UpdateRecipe_WhenUpdateAsyncReturnsArgumentNullException_ReturnFalse()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.SaveChangesAsync()).ReturnsAsync(true);
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.RecipeRepository.UpdateAsync(It.IsAny<int>(), It.IsAny<Recipe>())).ThrowsAsync(new ArgumentNullException("Recipe not found"));
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.DishesTypeRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new DishesType());

            var RecipeService = new RecipeService(_mockUnitOfWork.Object, _mockLogger.Object);

            bool result = await RecipeService.Update(1, recipeData);

            Assert.IsTrue(!result, "Recipe shouldn't update successfully");

            TestLoggerMethods(
               logErrorCount: 0,
               logErrorExCount: 1,
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
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.DishesTypeRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new DishesType());

            var RecipeService = new RecipeService(_mockUnitOfWork.Object, _mockLogger.Object);

            bool result = await RecipeService.Update(1, recipeData);

            Assert.IsTrue(result, "Recipe should update successfully");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 1,
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

            Assert.IsTrue(result, "Recipe deletion shouldn't fail");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 1,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task AddRecipe_WhenRecipeIsOk_ReturnTrue()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.RecipeRepository.AddAsync(It.IsAny<Recipe>()));
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.DishesTypeRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new DishesType());
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.SaveChangesAsync()).ReturnsAsync(true);

            var RecipeService = new RecipeService(_mockUnitOfWork.Object, _mockLogger.Object);

            var result = await RecipeService.Create(recipeData);

            Assert.AreNotEqual(-1,result, "Recipe creation shouldn't fail");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 1,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task AddRecipe_WhenRecipeIsNull_ThrowArgumentNullException()
        {
            var RecipeService = new RecipeService(_mockUnitOfWork.Object, _mockLogger.Object);

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => RecipeService.Create(null));

            TestLoggerMethods(
                logErrorCount: 1,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task AddRecipe_WhenDishTypeIsNotFound_ReturnFalse()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.RecipeRepository.AddAsync(It.IsAny<Recipe>()));
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.DishesTypeRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(() => null);
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.SaveChangesAsync()).ReturnsAsync(true);

            var RecipeService = new RecipeService(_mockUnitOfWork.Object, _mockLogger.Object);
            
            var result = await RecipeService.Create(recipeData);

            Assert.AreEqual(0,result, "Recipe creation shouldn fail");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 1,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }
    }
}
