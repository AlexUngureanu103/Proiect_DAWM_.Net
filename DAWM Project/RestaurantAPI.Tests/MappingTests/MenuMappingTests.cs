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
                ImageUrl = "test"
            };
            menu = new Menu
            {
                Id = 1,
                Name = "test",
                Price = 15,
                MenuItems = new(),
                ImageUrl = "test"
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
            Assert.AreEqual(mappedMenu.ImageUrl, menuData.ImageUrl, "Resulted Menu is not the same ");
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
            Assert.IsNotNull(menuInfo.RecipesIds, "Resulted Menu Info  recipes Id's is not null");
            Assert.IsNotNull(menuInfo.ImageUrl, "Resulted Menu Info  recipes Id's is not null");
        }

        [TestMethod]
        public void MapToMenuInfos_WhenMenuIsNotNull_ReturnMenuInfo()
        {
            menu.MenuItems = new List<MenuItem>() { };
            var menuInfo = MenuMapping.MapToMenuInfos(menu);

            Assert.IsNotNull(menuInfo, "User shouldn't be null when dto is null");

            Assert.AreEqual(menuInfo.Name, menu.Name, "Resulted Menu Info is not the same ");
            Assert.AreEqual(menuInfo.ImageUrl, menu.ImageUrl, "Resulted Menu Info is not the same ");
            Assert.AreEqual(menuInfo.Price, menu.Price, "Resulted Menu Info is not the same ");
            Assert.IsNotNull(menuInfo.RecipesIds, "Resulted Menu Info recipes Id's is null");
            Assert.AreEqual(menuInfo.RecipesIds.Count, menu.MenuItems.Count, "Recipes Id's count should be the same ad menu items count");
        }

        [TestMethod]
        public void MapToMenuOrderInfos_WhenMenuIsNull_ReturnNull()
        {
            var menuInfo = MenuMapping.MapToMenuInfos(null, 2);

            Assert.IsNull(menuInfo, "Menu should be null when dto is null");
        }

        [TestMethod]
        public void MapToMenuOrderInfos_WhenMenuIsNotNullButMenuItemsIsNull_ReturnMenuInfoWithNullRecipesIds()
        {
            var menuInfo = MenuMapping.MapToMenuInfos(menu, 2);

            Assert.IsNotNull(menuInfo, "User shouldn't be null when dto is null");

            Assert.AreEqual(menuInfo.Name, menu.Name, "Resulted Menu Info is not the same ");
            Assert.AreEqual(menuInfo.Price, menu.Price, "Resulted Menu Info is not the same ");
            Assert.IsNotNull(menuInfo.RecipesIds, "Resulted Menu Info  recipes Id's is not null");
            Assert.IsNotNull(menuInfo.ImageUrl, "Resulted Menu Info  recipes Id's is not null");
        }

        [TestMethod]
        public void MapToMenuOrderInfos_WhenMenuIsNotNull_ReturnMenuInfo()
        {
            menu.MenuItems = new List<MenuItem>() { };
            var menuInfo = MenuMapping.MapToMenuInfos(menu, 2);

            Assert.IsNotNull(menuInfo, "User shouldn't be null when dto is null");

            Assert.AreEqual(menuInfo.Name, menu.Name, "Resulted Menu Info is not the same ");
            Assert.AreEqual(menuInfo.ImageUrl, menu.ImageUrl, "Resulted Menu Info is not the same ");
            Assert.AreEqual(menuInfo.Price, menu.Price, "Resulted Menu Info is not the same ");
            Assert.IsNotNull(menuInfo.RecipesIds, "Resulted Menu Info recipes Id's is null");
            Assert.AreEqual(menuInfo.RecipesIds.Count, menu.MenuItems.Count, "Recipes Id's count should be the same ad menu items count");
        }

        [TestMethod]
        public void MapToMenuItemDto_WhenMenuItemIsNull_ReturnNull()
        {
            var mappedMenuItem = MenuMapping.MapToMenuItemDto(null);

            Assert.IsNull(mappedMenuItem, "MenuItem should be null when dto is null");
        }

        [TestMethod]
        public void MapToMenuItemDto_WhenMenuItemIsNotNull_ReturnNull()
        {
            var menuItem = new MenuItem
            {
                Id = 1,
                Menu = new Menu(),
                MenuId = 1,
                Recipe = new Recipe(),
                RecipeId = 1,
            };
            var mappedMenuItem = MenuMapping.MapToMenuItemDto(menuItem);

            Assert.IsNotNull(mappedMenuItem, "MenuItem shouldn't be null when dto is null");
            Assert.AreEqual(mappedMenuItem.MenuId, menuItem.MenuId, "MenuItem shouldn't be null when dto is null");
            Assert.AreEqual(mappedMenuItem.RecipeId, menuItem.RecipeId, "MenuItem shouldn't be null when dto is null");
        }
    }
}
