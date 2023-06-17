using RestaurantAPI.Domain.Dtos.MenuDtos;
using RestaurantAPI.Domain.Mapping;
using RestaurantAPI.Domain.Models.MenuRelated;

namespace RestaurantAPI.Tests.MappingTests
{
    [TestClass]
    public class MenuMappingTests
    {
        private CreateOrUpdateMenu menuData;
        private Menu menu;

        [TestInitialize]
        public void TestInitialize()
        {
            menuData = new CreateOrUpdateMenu
            {
                Name = "test",
                Price = 12,
            };
            menu = new Menu
            {
                Id = 1,
                Name = "test",
                Price = 15,
                MenuItems = new()
            };
        }

        [TestCleanup]
        public void TestCleanup()
        {
            menuData = null;
            menu = null;
        }

        [TestMethod]
        public void MapToMenu_WhenMenuDataIsNull_ReturnNull()
        {
            var mappedMenu = MenuMapping.MapToMenu(null);

            Assert.IsNull(mappedMenu, "Menu should be null when dto is null");
        }

        [TestMethod]
        public void MapToMenu_WhenMenuDataIsNotNull_ReturnMenu()
        {
            var mappedMenu = MenuMapping.MapToMenu(menuData);

            Assert.IsNotNull(mappedMenu, "User shouldn't be null when dto is null");

            Assert.AreEqual(mappedMenu.Name, menuData.Name, "Resulted Menu is not the same ");
            Assert.AreEqual(mappedMenu.Price, menuData.Price, "Resulted Menu is not the same ");
        }

        [TestMethod]
        public void MapToMenuInfos_WhenMenuIsNull_ReturnNull()
        {
            var menuInfo = MenuMapping.MapToMenuInfos(null);

            Assert.IsNull(menuInfo, "Menu should be null when dto is null");
        }

        [TestMethod]
        public void MapToMenuInfos_WhenMenuIsNotNullButMenuItemsIsNull_ReturnMenuInfoWithNullRecipesIds()
        {
            var menuInfo = MenuMapping.MapToMenuInfos(menu);

            Assert.IsNotNull(menuInfo, "User shouldn't be null when dto is null");

            Assert.AreEqual(menuInfo.Name, menu.Name, "Resulted Menu Info is not the same ");
            Assert.AreEqual(menuInfo.Price, menu.Price, "Resulted Menu Info is not the same ");
            Assert.IsNotNull(menuInfo.RecipiesIds, "Resulted Menu Info  recipes Id's is not null");
        }

        [TestMethod]
        public void MapToMenuInfos_WhenMenuIsNotNull_ReturnMenuInfo()
        {
            menu.MenuItems = new List<MenuItem>() { };
            var menuInfo = MenuMapping.MapToMenuInfos(menu);

            Assert.IsNotNull(menuInfo, "User shouldn't be null when dto is null");

            Assert.AreEqual(menuInfo.Name, menu.Name, "Resulted Menu Info is not the same ");
            Assert.AreEqual(menuInfo.Price, menu.Price, "Resulted Menu Info is not the same ");
            Assert.IsNotNull(menuInfo.RecipiesIds, "Resulted Menu Info recipes Id's is null");
            Assert.AreEqual(menuInfo.RecipiesIds.Count, menu.MenuItems.Count, "Recipes Id's count should be the same ad menu items count");
        }
    }
}
