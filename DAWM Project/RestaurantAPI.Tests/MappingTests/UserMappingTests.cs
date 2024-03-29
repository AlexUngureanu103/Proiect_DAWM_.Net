﻿using RestaurantAPI.Domain.Dtos.UserDtos;
using RestaurantAPI.Domain.Mapping;
using RestaurantAPI.Domain.Models.Users;

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
        public void MapToUser_WhenUserIsNotNull_ReturnUser()
        {
            var user = UserMapping.MapToUser(userData);

            Assert.IsNotNull(user, "User shouldn't be null when dto is not null");

            Assert.AreEqual(user.Email, userData.Email, "Resulted user is not the same ");
            Assert.AreEqual(user.PasswordHash, userData.Password, "Resulted user is not the same ");
            Assert.AreEqual(user.FirstName, userData.FirstName, "Resulted user is not the same ");
            Assert.AreEqual(user.LastName, userData.LastName, "Resulted user is not the same ");
        }

        [TestMethod]
        public void MapToUserPublicData_WhenUserIsNull_ReturnNull()
        {
            var userPublicData = UserMapping.MapToUserPublicData(null);

            Assert.IsNull(userPublicData, "User should be null when dto is null");
        }

        [TestMethod]
        public void MapToUserPublicData_WhenUserIsNotNull_ReturnUserPublicData()
        {
            User user = new User
            {
                Email = "test",
                FirstName = "test",
                LastName = "test",
                PasswordHash = "test",
                Role = Domain.Enums.Role.Admin
            };
            
            var userPublicData = UserMapping.MapToUserPublicData(user);

            Assert.IsNotNull(userPublicData, "User shouldn't be null when dto is not null");

            Assert.AreEqual(userPublicData.Email, user.Email, "Resulted user is not the same ");
            Assert.AreEqual(userPublicData.FirstName, user.FirstName, "Resulted user is not the same ");
            Assert.AreEqual(userPublicData.LastName, user.LastName, "Resulted user is not the same ");
        }
    }
}
