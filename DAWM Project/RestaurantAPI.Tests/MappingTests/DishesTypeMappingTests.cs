using RestaurantAPI.Domain.Dtos.DishesTypeDtos;
using RestaurantAPI.Domain.Mapping;

namespace RestaurantAPI.Tests.MappingTests
{
    [TestClass]
    public class DishesTypeMappingTests
    {
        private CreateOrUpdateDishesType dishesType;

        [TestInitialize]
        public void TestInitialize()
        {
            dishesType = new CreateOrUpdateDishesType
            {
                Name = "test"
            };
        }

        [TestCleanup]
        public void TestCleanup()
        {
            dishesType = null;
        }

        [TestMethod]
        public void MapToDishesType_WhenDishTypeIsNull_ReturnNull()
        {
            var dishType = IngredientMapping.MapToIngredient(null);

            Assert.IsNull(dishType, "DishType should be null when dto is null");
        }

        [TestMethod]
        public void MapToUser_WhenDishTypeIsNotNull_ReturnDishType()
        {
            var dishType = DishesTypeMapping.MapToDishesType(dishesType);

            Assert.IsNotNull(dishType, "DishType shouldn't be null when dto is null");

            Assert.AreEqual(dishType.Name, dishesType.Name, "Resulted DishType is not the same ");
        }
    }
}
