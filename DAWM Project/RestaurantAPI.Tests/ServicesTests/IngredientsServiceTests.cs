﻿using Core.Services;
using Moq;
using RestaurantAPI.Domain;
using RestaurantAPI.Domain.Dtos.IngredientDtos;
using RestaurantAPI.Domain.Models.MenuRelated;
using RestaurantAPI.Exceptions;

namespace RestaurantAPI.Tests.ServicesTests
{
    [TestClass]
    public class IngredientsServiceTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IDataLogger> _mockLogger;
        private CreateOrUpdateIngredient ingredientData;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockUnitOfWork = new();
            _mockLogger = new();
            ingredientData = new()
            {
                Name = "test",
                TotalWeight = 10
            };
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            _mockUnitOfWork = null;
            _mockLogger = null;
            ingredientData = null;
        }

        [TestMethod]
        public void InstantiatingIngredientsService_WhenUnitOfWorkIsNull_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new IngredientsService(null, _mockLogger.Object));
        }

        [TestMethod]
        public void InstantiatingIngredientsService_WhenIDataLoggerIsNull_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new IngredientsService(_mockUnitOfWork.Object, null));
        }

        [TestMethod]
        public async Task GetAllIngredients_WhenCalled_ReturnsAllIngredients()
        {
            List<Ingredient> ingredients = new()
            {
                new Ingredient
                {
                    Id = 1,
                    Name = "Ingredient 1",
                    TotalWeight=214
                },
                new Ingredient
                {
                    Id = 2,
                    Name = "Ingredient 2",
                    TotalWeight=12
                }
            };

            _mockUnitOfWork.Setup(x => x.IngredientRepository.GetAllAsync()).ReturnsAsync(ingredients);

            var ingredientsService = new IngredientsService(_mockUnitOfWork.Object, _mockLogger.Object);

            var result = await ingredientsService.GetAllIngredients();

            Assert.IsNotNull(result, "Ingredient list shouldn't be null");
            Assert.AreEqual(2, result.Count(), "Ingredient list should only contain 2 elements");

            _mockLogger.Verify(log => log.LogError(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
            _mockLogger.Verify(log => log.LogWarn(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogInfo(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogDebug(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task GetIngredientById_WhenIngredientIsFound_ReturnIngredient()
        {
            List<Ingredient> ingredients = new()
            {
                new Ingredient
                {
                    Id = 1,
                    Name = "Ingredient 1",
                    TotalWeight=214
                },
                new Ingredient
                {
                    Id = 2,
                    Name = "Ingredient 2",
                    TotalWeight=12
                }
            };
            int id = 1;

            _mockUnitOfWork.Setup(x => x.IngredientRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(ingredients[1]);

            var ingredientsService = new IngredientsService(_mockUnitOfWork.Object, _mockLogger.Object);

            var result = await ingredientsService.GetIngredient(id);

            Assert.IsNotNull(result, "Ingredient should be found");

            _mockLogger.Verify(log => log.LogError(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
            _mockLogger.Verify(log => log.LogWarn(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogInfo(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogDebug(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task GetIngredientById_WhenIngredientIsNotFound_ReturnNull()
        {
            List<Ingredient> ingredients = new()
            {
                new Ingredient
                {
                    Id = 1,
                    Name = "Ingredient 1",
                    TotalWeight=214
                },
                new Ingredient
                {
                    Id = 2,
                    Name = "Ingredient 2",
                    TotalWeight=12
                }
            };
            int id = 351;

            _mockUnitOfWork.Setup(x => x.IngredientRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return null;
            });

            var ingredientsService = new IngredientsService(_mockUnitOfWork.Object, _mockLogger.Object);

            var result = await ingredientsService.GetIngredient(id);

            Assert.IsNull(result, "Ingredient shouldn't be found");

            _mockLogger.Verify(log => log.LogError(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
            _mockLogger.Verify(log => log.LogWarn(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogInfo(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogDebug(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task UpdateIngredient_WhenIngredientIsNull_ThrowArguemntNullException()
        {
            var ingredientsService = new IngredientsService(_mockUnitOfWork.Object, _mockLogger.Object);

            Assert.ThrowsExceptionAsync<ArgumentNullException>(() => ingredientsService.UpdateIngredient(1, null));

            _mockLogger.Verify(log => log.LogError(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
            _mockLogger.Verify(log => log.LogWarn(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogInfo(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogDebug(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task UpdateIngredient_WhenIngredientIsOk_ReturnTrue()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.SaveChangesAsync()).ReturnsAsync(true);
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.IngredientRepository.UpdateAsync(It.IsAny<int>(), It.IsAny<Ingredient>()));

            var ingredientsService = new IngredientsService(_mockUnitOfWork.Object, _mockLogger.Object);

            bool result = await ingredientsService.UpdateIngredient(1, ingredientData);

            Assert.IsTrue(result, "Ingredient should be updates");

            _mockLogger.Verify(log => log.LogError(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
            _mockLogger.Verify(log => log.LogWarn(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogInfo(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogDebug(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task DeleteIngredient_WhenIngredientIsNotOk_ReturnFalse()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.IngredientRepository.DeleteAsync(It.IsAny<int>())).ThrowsAsync(new EntityNotFoundException());

            var ingredientsService = new IngredientsService(_mockUnitOfWork.Object, _mockLogger.Object);

            bool result = await ingredientsService.DeleteIngredient(1);

            Assert.IsTrue(!result, "Ingredient deletion should fail");

            _mockLogger.Verify(log => log.LogError(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
            _mockLogger.Verify(log => log.LogWarn(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogInfo(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogDebug(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task DeleteIngredient_WhenIngredientIsOk_ReturnTrue()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.IngredientRepository.DeleteAsync(It.IsAny<int>()));
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.SaveChangesAsync()).ReturnsAsync(true);

            var ingredientsService = new IngredientsService(_mockUnitOfWork.Object, _mockLogger.Object);

            bool result = await ingredientsService.DeleteIngredient(1);

            Assert.IsTrue(result, "Ingredient deletion should fail");

            _mockLogger.Verify(log => log.LogError(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
            _mockLogger.Verify(log => log.LogWarn(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogInfo(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogDebug(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task AddIngredient_WhenIngredientIsOk_ReturnTrue()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.IngredientRepository.AddAsync(It.IsAny<Ingredient>()));
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.SaveChangesAsync()).ReturnsAsync(true);

            var ingredientsService = new IngredientsService(_mockUnitOfWork.Object, _mockLogger.Object);

            bool result = await ingredientsService.AddIngredient(ingredientData);

            Assert.IsTrue(result, "Ingredient creation shouldn't fail");

            _mockLogger.Verify(log => log.LogError(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
            _mockLogger.Verify(log => log.LogWarn(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogInfo(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogDebug(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task AddIngredient_WhenIngredientIsNull_ThrowArgumentNullException()
        {
            var ingredientsService = new IngredientsService(_mockUnitOfWork.Object, _mockLogger.Object);

            Assert.ThrowsExceptionAsync<ArgumentNullException>(() => ingredientsService.AddIngredient(null));

            _mockLogger.Verify(log => log.LogError(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
            _mockLogger.Verify(log => log.LogWarn(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogInfo(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogDebug(It.IsAny<string>()), Times.Never);
        }
    }
}