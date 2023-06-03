using RestaurantAPI.Domain.Dtos.UserDtos;
using RestaurantAPI.Domain.Mapping;

namespace RestaurantAPI.Tests.MappingTests
{
    [TestClass]
    public class UserMappingTests
    {
        private CreateOrUpdateUser userData;

        [TestInitialize]
        public void TestInitialize()
        {
            userData = new CreateOrUpdateUser
            {
                Email = "test",
                FirstName = "test",
                LastName = "test",
                Password = "test",
                Role = Domain.Enums.Role.Admin
            };
        }

        [TestCleanup]
        public void TestCleanup()
        {
            userData = null;
        }

        [TestMethod]
        public void MapToUser_WhenUserIsNull_ReturnNull()
        {
            var user = UserMapping.MapToUser(null);

            Assert.IsNull(user, "User should be null when dto is null");
        }

        [TestMethod]
        public void MapToUser_WhenUserIsNull_ReturnUser()
        {
            var user = UserMapping.MapToUser(userData);

            Assert.IsNotNull(user, "User shouldn't be null when dto is null");

            Assert.AreEqual(user.Email, userData.Email, "Resulted user is not the same ");
            Assert.AreEqual(user.PasswordHash, userData.Password, "Resulted user is not the same ");
            Assert.AreEqual(user.Role, userData.Role, "Resulted user is not the same ");
            Assert.AreEqual(user.FirstName, userData.FirstName, "Resulted user is not the same ");
            Assert.AreEqual(user.LastName, userData.LastName, "Resulted user is not the same ");
        }
    }
}
