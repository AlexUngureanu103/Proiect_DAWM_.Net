using Core.Services;
using Moq;
using RestaurantAPI.Domain;
using RestaurantAPI.Domain.Dtos.UserDtos;
using RestaurantAPI.Domain.Enums;
using RestaurantAPI.Domain.Models.Users;

namespace RestaurantAPI.Tests
{
    [TestClass]
    public class UserServiceTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IAuthorizationService> _mockAuthorizationService;
        private Mock<IDataLogger> _mockLogger;
        private CreateOrUpdateUser userData;
        private LoginDto loginData;

        [TestInitialize]
        public void TestInitializa()
        {
            _mockUnitOfWork = new();
            _mockAuthorizationService = new();
            _mockLogger = new();
            userData = new()
            {
                Email = "test@test.ro",
                FirstName = "test",
                LastName = "test",
                Password = "test",
                Role = Role.Guest
            };
            loginData = new()
            {
                Email = "string",
                Password = "string"
            };
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            _mockUnitOfWork = null;
            _mockAuthorizationService = null;
            _mockLogger = null;
            userData = null;
            loginData = null;
        }

        [TestMethod]
        public void InstantiatingUsersService_WhenUnitOfWorkIsNull_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new UsersService(null, _mockAuthorizationService.Object, _mockLogger.Object));
        }

        [TestMethod]
        public void InstantiatingUsersService_WhenAuthorizationServiceIsNull_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new UsersService(_mockUnitOfWork.Object, null, _mockLogger.Object));
        }

        [TestMethod]
        public void InstantiatingUsersService_WhenDataLoggerIsNull_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new UsersService(_mockUnitOfWork.Object, _mockAuthorizationService.Object, null));
        }

        [TestMethod]
        public async Task HavingUserServiceInstance_WhenRegisterDataIsNull_ReturnFalse()
        {
            await TestRegisterFails(null);
        }

        [TestMethod]
        public async Task HavingUserServiceInstance_WhenEmailIsNull_ReturnFalse()
        {
            userData.Email = null;

            await TestRegisterFails(userData);
        }

        [TestMethod]
        public async Task HavingUserServiceInstance_WhenFirstNameIsNull_ReturnFalse()
        {
            userData.FirstName = null;

            await TestRegisterFails(userData);
        }

        [TestMethod]
        public async Task HavingUserServiceInstance_WhenLastNameIsNull_ReturnFalse()
        {
            userData.LastName = null;

            await TestRegisterFails(userData);
        }

        [TestMethod]
        public async Task HavingUserServiceInstance_WhenPasswordIsNull_ReturnFalse()
        {
            userData.Password = null;

            await TestRegisterFails(userData);
        }

        [TestMethod]
        public async Task HavingUserServiceInstance_WhenEmailsAlreadyExists_ReturnFalse()
        {
            _mockUnitOfWork.Setup(c => c.UsersRepository.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(() => new User());
            UsersService userService = new UsersService(_mockUnitOfWork.Object, _mockAuthorizationService.Object, _mockLogger.Object);

            bool result = await userService.Register(userData);

            Assert.IsTrue(!result, "Register procress should fail  when email already exists");

            _mockLogger.Verify(log => log.LogError(It.IsAny<string>()), Times.Once);
            _mockLogger.Verify(log => log.LogWarn(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogInfo(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogDebug(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task HavingUserServiceInstance_WhenRegisterDataIsOk_ReturnTrue()
        {
            _mockUnitOfWork.Setup(c => c.UsersRepository.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(() => null);
            _mockUnitOfWork.Setup(c => c.SaveChangesAsync()).ReturnsAsync(() => true);
            UsersService userService = new UsersService(_mockUnitOfWork.Object, _mockAuthorizationService.Object, _mockLogger.Object);

            bool result = await userService.Register(userData);

            Assert.IsTrue(result, "Register procress shouldn't fail  when registration is successful");

            _mockLogger.Verify(log => log.LogError(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogWarn(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogInfo(It.IsAny<string>()), Times.Once);
            _mockLogger.Verify(log => log.LogDebug(It.IsAny<string>()), Times.Never);
        }

        private async Task TestRegisterFails(CreateOrUpdateUser userData)
        {
            UsersService userService = new UsersService(_mockUnitOfWork.Object, _mockAuthorizationService.Object, _mockLogger.Object);
            bool result = await userService.Register(userData);

            Assert.IsTrue(!result, "Register procress should fail  when register data is null");

            _mockLogger.Verify(log => log.LogWarn(It.IsAny<string>()), Times.Once);
            _mockLogger.Verify(log => log.LogInfo(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogDebug(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogError(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task HavingUserServiceInstance_WhenValidatingCredentialsAndAccountNotFound_ReturnEmptyString()
        {
            _mockUnitOfWork.Setup(c => c.UsersRepository.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(() => null);
            UsersService userService = new UsersService(_mockUnitOfWork.Object, _mockAuthorizationService.Object, _mockLogger.Object);

            string result = await userService.ValidateCredentials(loginData);

            Assert.IsTrue(string.IsNullOrEmpty(result), "Register procress shouldn't fail  when registration is successful");

            _mockLogger.Verify(log => log.LogError(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogWarn(It.IsAny<string>()), Times.Once);
            _mockLogger.Verify(log => log.LogInfo(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogDebug(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task HavingUserServiceInstance_WhenValidatingCredentialsAndWrongPassword_ReturnEmptyString()
        {
            _mockUnitOfWork.Setup(c => c.UsersRepository.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(() => new User());
            _mockAuthorizationService.Setup(c => c.VerifyHashedPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(() => false);
            UsersService userService = new UsersService(_mockUnitOfWork.Object, _mockAuthorizationService.Object, _mockLogger.Object);

            string result = await userService.ValidateCredentials(loginData);

            Assert.IsTrue(string.IsNullOrEmpty(result), "Register procress shouldn't fail  when registration is successful");

            _mockLogger.Verify(log => log.LogError(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogWarn(It.IsAny<string>()), Times.Once);
            _mockLogger.Verify(log => log.LogInfo(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogDebug(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task HavingUserServiceInstance_WhenValidatingCredentialsAndPasswordIsOk_ReturnJwtToken()
        {
            _mockUnitOfWork.Setup(c => c.UsersRepository.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(() => new User());
            _mockAuthorizationService.Setup(c => c.VerifyHashedPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(() => true);
            _mockAuthorizationService.Setup(c => c.GetToken(It.IsAny<User>(), It.IsAny<string>())).Returns(() => "token");
            UsersService userService = new UsersService(_mockUnitOfWork.Object, _mockAuthorizationService.Object, _mockLogger.Object);

            string result = await userService.ValidateCredentials(loginData);

            Assert.IsTrue(!string.IsNullOrEmpty(result), "Register procress shouldn't fail  when registration is successful");

            _mockLogger.Verify(log => log.LogError(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogWarn(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogInfo(It.IsAny<string>()), Times.Once);
            _mockLogger.Verify(log => log.LogDebug(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task HavingUserServiceInstance_WhenUpdatingUserAndEmailAlreadyIsRegistered_ReturnFalse()
        {
            _mockUnitOfWork.Setup(c => c.UsersRepository.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(() => new User() { Id = 512 });

            UsersService userService = new UsersService(_mockUnitOfWork.Object, _mockAuthorizationService.Object, _mockLogger.Object);

            bool result = await userService.UpdateUserDetails(1, userData);

            Assert.IsTrue(!result, "Register procress shouldn't fail  when registration is successful");

            _mockLogger.Verify(log => log.LogError(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogWarn(It.IsAny<string>()), Times.Once);
            _mockLogger.Verify(log => log.LogInfo(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogDebug(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task HavingUserServiceInstance_WhenUpdatingUserWithSuccess_ReturnTrue()
        {
            int userId = 1;
            _mockUnitOfWork.Setup(c => c.UsersRepository.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(() => new User() { Id = userId });

            UsersService userService = new UsersService(_mockUnitOfWork.Object, _mockAuthorizationService.Object, _mockLogger.Object);

            bool result = await userService.UpdateUserDetails(userId, userData);

            Assert.IsTrue(result, "Register procress shouldn't fail  when registration is successful");

            _mockLogger.Verify(log => log.LogError(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogWarn(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogInfo(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogDebug(It.IsAny<string>()), Times.Never);
        }
    }
}