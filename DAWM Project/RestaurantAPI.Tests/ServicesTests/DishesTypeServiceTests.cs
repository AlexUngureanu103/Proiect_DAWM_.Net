using Core.Services;
using Moq;
using RestaurantAPI.Domain;
using RestaurantAPI.Domain.Dtos.DishesTypeDtos;
using RestaurantAPI.Domain.Models.MenuRelated;
using RestaurantAPI.Exceptions;

namespace RestaurantAPI.Tests.ServicesTests
{
    [TestClass]
    public class DishesTypeServiceTests : LoggerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private CreateOrUpdateDishesType dishTypeData;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockUnitOfWork = new();
            _mockLogger = new();
            dishTypeData = new()
            {
                Name = "test"
            };
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            _mockUnitOfWork = null;
            _mockLogger = null;
            dishTypeData = null;
        }

        [TestMethod]
        public void InstantiatingDishesTypeService_WhenUnitOfWorkIsNull_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new DishesTypeService(null, _mockLogger.Object));
        }

        [TestMethod]
        public void InstantiatingDishesTypeService_WhenIDataLoggerIsNull_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new DishesTypeService(_mockUnitOfWork.Object, null));
        }

        [TestMethod]
        public async Task GetAllDishesType_WhenCalled_ReturnsAllDishesType()
        {
            List<DishesType> DishesType = new()
            {
                new DishesType
                {
                    Id = 1,
                    Name = "type 1"
                },
                new DishesType
                {
                    Id = 2,
                    Name = "type 2"
                }
            };

            _mockUnitOfWork.Setup(x => x.DishesTypeRepository.GetAllAsync()).ReturnsAsync(DishesType);

            var DishesTypeService = new DishesTypeService(_mockUnitOfWork.Object, _mockLogger.Object);

            var result = await DishesTypeService.GetAll();

            Assert.IsNotNull(result, "DishType list shouldn't be null");
            Assert.AreEqual(2, result.Count(), "DishType list should only contain 2 elements");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task GetDishTypeById_WhenDishTypeIsFound_ReturnDishType()
        {
            List<DishesType> DishesType = new()
            {
                new DishesType
                {
                    Id = 1,
                    Name = "type 1"
                },
                new DishesType
                {
                    Id = 2,
                    Name = "type 2"
                }
            };
            int id = 1;

            _mockUnitOfWork.Setup(x => x.DishesTypeRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(DishesType[1]);

            var DishesTypeService = new DishesTypeService(_mockUnitOfWork.Object, _mockLogger.Object);

            var result = await DishesTypeService.GetById(id);

            Assert.IsNotNull(result, "DishType should be found");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task GetDishTypeById_WhenDishTypeIsNotFound_ReturnNull()
        {
            List<DishesType> DishesType = new()
            {
                new DishesType
                {
                    Id = 1,
                    Name = "type 1"
                },
                new DishesType
                {
                    Id = 2,
                    Name = "type 2"
                }
            };
            int id = 351;

            _mockUnitOfWork.Setup(x => x.DishesTypeRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return null;
            });

            var DishesTypeService = new DishesTypeService(_mockUnitOfWork.Object, _mockLogger.Object);

            var result = await DishesTypeService.GetById(id);

            Assert.IsNull(result, "DishType shouldn't be found");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task UpdateDishType_WhenDishTypeIsNull_ThrowArguemntNullException()
        {
            var DishesTypeService = new DishesTypeService(_mockUnitOfWork.Object, _mockLogger.Object);

            Assert.ThrowsExceptionAsync<ArgumentNullException>(() => DishesTypeService.Update(1, null));

            TestLoggerMethods(
                logErrorCount: 1,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task UpdateDishType_WhenDishTypeIsOk_ReturnTrue()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.SaveChangesAsync()).ReturnsAsync(true);
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.DishesTypeRepository.UpdateAsync(It.IsAny<int>(), It.IsAny<DishesType>()));

            var DishesTypeService = new DishesTypeService(_mockUnitOfWork.Object, _mockLogger.Object);

            bool result = await DishesTypeService.Update(1, dishTypeData);

            Assert.IsTrue(result, "DishType should be updates");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task DeleteDishType_WhenDishTypeIsNotOk_ReturnFalse()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.DishesTypeRepository.DeleteAsync(It.IsAny<int>())).ThrowsAsync(new EntityNotFoundException());

            var DishesTypeService = new DishesTypeService(_mockUnitOfWork.Object, _mockLogger.Object);

            bool result = await DishesTypeService.Delete(1);

            Assert.IsTrue(!result, "DishType deletion should fail");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 1,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task DeleteDishType_WhenDishTypeIsOk_ReturnTrue()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.DishesTypeRepository.DeleteAsync(It.IsAny<int>()));
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.SaveChangesAsync()).ReturnsAsync(true);

            var DishesTypeService = new DishesTypeService(_mockUnitOfWork.Object, _mockLogger.Object);

            bool result = await DishesTypeService.Delete(1);

            Assert.IsTrue(result, "DishType deletion should fail");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task AddDishType_WhenDishTypeIsOk_ReturnTrue()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.DishesTypeRepository.AddAsync(It.IsAny<DishesType>()));
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.SaveChangesAsync()).ReturnsAsync(true);

            var DishesTypeService = new DishesTypeService(_mockUnitOfWork.Object, _mockLogger.Object);

            bool result = await DishesTypeService.Create(dishTypeData);

            Assert.IsTrue(result, "DishType creation shouldn't fail");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task AddDishType_WhenDishTypeIsNull_ThrowArgumentNullException()
        {
            var DishesTypeService = new DishesTypeService(_mockUnitOfWork.Object, _mockLogger.Object);

            Assert.ThrowsExceptionAsync<ArgumentNullException>(() => DishesTypeService.Create(null));

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
