using Core.Services;
using Moq;
using RestaurantAPI.Domain;
using RestaurantAPI.Domain.Dtos.MenuDtos;
using RestaurantAPI.Domain.Models.MenuRelated;
using RestaurantAPI.Exceptions;

namespace RestaurantAPI.Tests.ServicesTests
{
    [TestClass]
    public class MenusServiceTests : LoggerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private CreateOrUpdateMenu menuData;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockUnitOfWork = new();
            _mockLogger = new();
            menuData = new()
            {
                Name = "test"
            };
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            _mockUnitOfWork = null;
            _mockLogger = null;
            menuData = null;
        }

        [TestMethod]
        public void InstantiatingMenusService_WhenUnitOfWorkIsNull_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new MenusService(null, _mockLogger.Object));
        }

        [TestMethod]
        public void InstantiatingMenusService_WhenIDataLoggerIsNull_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new MenusService(_mockUnitOfWork.Object, null));
        }

        [TestMethod]
        public async Task GetAllMenus_WhenCalled_ReturnsAllMenus()
        {
            List<Menu> Menus = new()
            {
                new Menu
                {
                    Id = 1,
                    Name = "Menu 1",
                    Price =15,
                    MenuItems = null
                },
                new Menu
                {
                    Id = 2,
                    Name = "Menu 2",
                    Price =125,
                    MenuItems = null
                }
            };

            _mockUnitOfWork.Setup(x => x.MenusRepository.GetAllAsync()).ReturnsAsync(Menus);

            var MenusService = new MenusService(_mockUnitOfWork.Object, _mockLogger.Object);

            var result = await MenusService.GetAllMenus();

            Assert.IsNotNull(result, "Menu list shouldn't be null");
            Assert.AreEqual(2, result.Count(), "Menu list should only contain 2 elements");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task GetMenuById_WhenMenuIsFound_ReturnMenu()
        {
            List<Menu> Menus = new()
            {
                new Menu
                {
                    Id = 1,
                    Name = "Menu 1",
                    Price =15,
                    MenuItems = null
                },
                new Menu
                {
                    Id = 2,
                    Name = "Menu 2",
                    Price =125,
                    MenuItems = null
                }
            };
            int id = 1;

            _mockUnitOfWork.Setup(x => x.MenusRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(Menus[1]);

            var MenusService = new MenusService(_mockUnitOfWork.Object, _mockLogger.Object);

            var result = await MenusService.GetMenuById(id);

            Assert.IsNotNull(result, "Menu should be found");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task GetMenuById_WhenMenuIsNotFound_ReturnNull()
        {
            List<Menu> Menus = new()
            {
                new Menu
                {
                    Id = 1,
                    Name = "Menu 1",
                    Price =15,
                    MenuItems = null
                },
                new Menu
                {
                    Id = 2,
                    Name = "Menu 2",
                    Price =125,
                    MenuItems = null
                }
            };
            int id = 351;

            _mockUnitOfWork.Setup(x => x.MenusRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return null;
            });

            var MenusService = new MenusService(_mockUnitOfWork.Object, _mockLogger.Object);

            var result = await MenusService.GetMenuById(id);

            Assert.IsNull(result, "Menu shouldn't be found");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task UpdateMenu_WhenMenuIsNull_ThrowArguemntNullException()
        {
            var MenusService = new MenusService(_mockUnitOfWork.Object, _mockLogger.Object);

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => MenusService.UpdateMenu(1, null));

            TestLoggerMethods(
                logErrorCount: 1,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task UpdateMenuMenu_WhenMenuIsOk_ReturnTrue()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.SaveChangesAsync()).ReturnsAsync(true);
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.MenusRepository.UpdateAsync(It.IsAny<int>(), It.IsAny<Menu>()));

            var MenusService = new MenusService(_mockUnitOfWork.Object, _mockLogger.Object);

            bool result = await MenusService.UpdateMenu(1, menuData);

            Assert.IsTrue(result, "Menu should  Update successfully");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task DeleteMenuMenu_WhenMenuIsNotOk_ReturnFalse()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.MenusRepository.DeleteAsync(It.IsAny<int>())).ThrowsAsync(new EntityNotFoundException());

            var MenusService = new MenusService(_mockUnitOfWork.Object, _mockLogger.Object);

            bool result = await MenusService.DeleteMenu(1);

            Assert.IsTrue(!result, "Menu deletion should fail");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 1,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task DeleteMenuMenu_WhenMenuIsOk_ReturnTrue()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.MenusRepository.DeleteAsync(It.IsAny<int>()));
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.SaveChangesAsync()).ReturnsAsync(true);

            var MenusService = new MenusService(_mockUnitOfWork.Object, _mockLogger.Object);

            bool result = await MenusService.DeleteMenu(1);

            Assert.IsTrue(result, "Menu deletion should not fail");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task AddMenu_WhenMenuIsOk_ReturnTrue()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.MenusRepository.AddAsync(It.IsAny<Menu>()));
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.SaveChangesAsync()).ReturnsAsync(true);

            var MenusService = new MenusService(_mockUnitOfWork.Object, _mockLogger.Object);

            bool result = await MenusService.AddMenu(menuData);

            Assert.IsTrue(result, "Menu creation shouldn't fail");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task AddMenu_WhenMenuIsNull_ThrowArgumentNullException()
        {
            var MenusService = new MenusService(_mockUnitOfWork.Object, _mockLogger.Object);

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => MenusService.AddMenu(null));

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task AddMenuItem_WhenMenuIsNotFound_ReturnFalse()
        {
            _mockUnitOfWork.Setup(x => x.MenusRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return null;
            });

            var MenusService = new MenusService(_mockUnitOfWork.Object, _mockLogger.Object);

            bool result = await MenusService.AddMenuItem(0, 1);

            Assert.IsFalse(result, "Menu shouldn't be found");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 1,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task AddMenuItem_WhenRecipeIsNotFound_ReturnFalse()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.MenusRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return new Menu();
            });
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.RecipeRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return null;
            });

            var MenusService = new MenusService(_mockUnitOfWork.Object, _mockLogger.Object);

            bool result = await MenusService.AddMenuItem(0, 1);

            Assert.IsFalse(result, "Recipe shouldn't be found");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 1,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task AddMenuItem_WhenEverythinIsOk_ReturnTrue()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.MenusRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return new Menu { MenuItems = new() };
            });
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.RecipeRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return new Recipe { Id = 1 };
            });
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.SaveChangesAsync()).ReturnsAsync(() => { return true; });

            var MenusService = new MenusService(_mockUnitOfWork.Object, _mockLogger.Object);

            bool result = await MenusService.AddMenuItem(0, 1);

            Assert.IsTrue(result, "Menu should update successfully");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 1,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task DeleteMenuItem_WhenMenuIsNotFound_ReturnFalse()
        {
            _mockUnitOfWork.Setup(x => x.MenusRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return null;
            });

            var MenusService = new MenusService(_mockUnitOfWork.Object, _mockLogger.Object);

            bool result = await MenusService.DeleteMenuItem(0, 1);

            Assert.IsFalse(result, "Menu shouldn't be found");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 1,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task DeleteMenuItem_WhenRecipeIsNotFound_ReturnFalse()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.MenusRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return new Menu { MenuItems = new() };
            });

            var MenusService = new MenusService(_mockUnitOfWork.Object, _mockLogger.Object);

            bool result = await MenusService.DeleteMenuItem(0, 1);

            Assert.IsFalse(result, "Recipe shouldn't be found");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 1,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task DeleteMenuItem_WhenEverythingIsOk_ReturnTrue()
        {
            int menuItemId = 1;
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.MenusRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return new Menu { MenuItems = new() { new MenuItem { RecipeId = menuItemId } } };
            });
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.SaveChangesAsync()).ReturnsAsync(() => { return true; });

            var MenusService = new MenusService(_mockUnitOfWork.Object, _mockLogger.Object);

            bool result = await MenusService.DeleteMenuItem(0, menuItemId);

            Assert.IsTrue(result, "Recipe shouldn't be found");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 1,
                logDebugCount: 0
                );
        }
    }
}
