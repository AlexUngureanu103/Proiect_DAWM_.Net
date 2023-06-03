using Core.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace RestaurantAPI.Tests.ServicesTests
{
    [TestClass]
    public class AuthorizationServiceTests
    {
        private Mock<IConfiguration> _mockConfiguration;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockConfiguration = new();
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            _mockConfiguration = null;
        }

        [TestMethod]
        public void InstantiatingAuthorizationService_WhenConfigurationIsNull_ThrowArgumentNullException()
        {

            Assert.ThrowsException<ArgumentNullException>(() => new AuthorizationService(null));
        }

        [TestMethod]
        public void HavingAuthorizationServiceInstance_WhenHashPasswordIsCalledAndPasswordIsNull_ThrowArgumentNullException()
        {
            AuthorizationService authorizationService = new(_mockConfiguration.Object);

            Assert.ThrowsException<ArgumentNullException>(() => authorizationService.HashPassword(null));
        }

        [TestMethod]
        public void HavingAuthorizationServiceInstance_WhenHashPasswordIsCalledAndPasswordIsOk_ReturnHashedPassword()
        {
            AuthorizationService authorizationService = new(_mockConfiguration.Object);

            string hashPassword = authorizationService.HashPassword("test");

            Assert.IsTrue(!string.IsNullOrEmpty(hashPassword), "The password should not be null");
        }

        [TestMethod]
        public void HavingAuthorizationServiceInstance_WhenVerifyHashPasswordAndHashedPasswordIsNull_ReturnFalse()
        {
            AuthorizationService authorizationService = new(_mockConfiguration.Object);

            bool result = authorizationService.VerifyHashedPassword(string.Empty, "test");

            Assert.IsTrue(!result, "The password should not match");
        }

        [TestMethod]
        public void HavingAuthorizationServiceInstance_WhenVerifyHashPasswordAndPasswordIsNull_ThrowsArgumentNullException()
        {
            AuthorizationService authorizationService = new(_mockConfiguration.Object);

            Assert.ThrowsException<ArgumentNullException>(() => authorizationService.VerifyHashedPassword("test", string.Empty));
        }

        [TestMethod]
        public void HavingAuthorizationServiceInstance_WhenPasswordMatch_ReturnTrue()
        {
            string password = "test";

            AuthorizationService authorizationService = new(_mockConfiguration.Object);

            string passwordHash = authorizationService.HashPassword(password);

            bool result = authorizationService.VerifyHashedPassword(passwordHash, password);

            Assert.IsTrue(result, "The password should match");
        }

        [TestMethod]
        public void HavingAuthorizationServiceInstance_WhenPasswordDoNotMatch_ReturnFalse()
        {
            string password = "test";

            AuthorizationService authorizationService = new(_mockConfiguration.Object);

            string passwordHash = authorizationService.HashPassword("NotTest");

            bool result = authorizationService.VerifyHashedPassword(passwordHash, password);

            Assert.IsTrue(!result, "The password should not match");
        }
    }
}
