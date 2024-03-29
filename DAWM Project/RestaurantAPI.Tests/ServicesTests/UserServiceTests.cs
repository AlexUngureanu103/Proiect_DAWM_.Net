using Core.Services;
using Moq;
using RestaurantAPI.Domain;
using RestaurantAPI.Domain.Dtos.UserDtos;
using RestaurantAPI.Domain.Enums;
using RestaurantAPI.Domain.Models.Users;

namespace RestaurantAPI.Tests.ServicesTests
{
    [TestClass]
    public class UserServiceTests : LoggerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IAuthorizationService> _mockAuthorizationService;
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
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.UsersRepository.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(() => new User());

            UsersService userService = new UsersService(_mockUnitOfWork.Object, _mockAuthorizationService.Object, _mockLogger.Object);

            bool result = await userService.Register(userData);

            Assert.IsTrue(!result, "Register procress should fail  when email already exists");

            TestLoggerMethods(
                logErrorCount: 1,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task HavingUserServiceInstance_WhenRegisterDataIsOk_ReturnTrue()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.UsersRepository.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(() => null);
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.SaveChangesAsync()).ReturnsAsync(() => true);

            UsersService userService = new UsersService(_mockUnitOfWork.Object, _mockAuthorizationService.Object, _mockLogger.Object);

            bool result = await userService.Register(userData);

            Assert.IsTrue(result, "Register procress shouldn't fail  when registration is successful");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 1,
                logDebugCount: 0
                );
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
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.UsersRepository.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(() => null);
            UsersService userService = new UsersService(_mockUnitOfWork.Object, _mockAuthorizationService.Object, _mockLogger.Object);

            string result = await userService.ValidateCredentials(loginData);

            Assert.IsTrue(string.IsNullOrEmpty(result), "Register procress should fail when account is not found");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 1,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task HavingUserServiceInstance_WhenValidatingCredentialsAndWrongPassword_ReturnEmptyString()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.UsersRepository.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(() => new User());
            _mockAuthorizationService.Setup(authService => authService.VerifyHashedPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(() => false);

            UsersService userService = new UsersService(_mockUnitOfWork.Object, _mockAuthorizationService.Object, _mockLogger.Object);

            string result = await userService.ValidateCredentials(loginData);

            Assert.IsTrue(string.IsNullOrEmpty(result), "Register procress should fail when password is wrong");
            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 1,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task HavingUserServiceInstance_WhenValidatingCredentialsAndPasswordIsOk_ReturnJwtToken()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.UsersRepository.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(() => new User { Role = Role.Guest });
            _mockAuthorizationService.Setup(authService => authService.VerifyHashedPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(() => true);
            _mockAuthorizationService.Setup(authService => authService.GetToken(It.IsAny<User>(), It.IsAny<string>())).Returns(() => "token");
            UsersService userService = new UsersService(_mockUnitOfWork.Object, _mockAuthorizationService.Object, _mockLogger.Object);

            string result = await userService.ValidateCredentials(loginData);

            Assert.IsTrue(!string.IsNullOrEmpty(result), "Register procress shouldn't fail when registration is successful");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 1,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task HavingUserServiceInstance_WhenValidatingCredentialsAndRolesIsUnautorized_ReturnEmptyString()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.UsersRepository.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(() => new User());
            _mockAuthorizationService.Setup(authService => authService.VerifyHashedPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(() => true);
            _mockAuthorizationService.Setup(authService => authService.GetToken(It.IsAny<User>(), It.IsAny<string>())).Returns(() => "token");
            UsersService userService = new UsersService(_mockUnitOfWork.Object, _mockAuthorizationService.Object, _mockLogger.Object);

            string result = await userService.ValidateCredentials(loginData);

            Assert.IsTrue(string.IsNullOrEmpty(result), "Register procress should fail when user is unautorized");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 1,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task HavingUserServiceInstance_WhenValidatingAdminCredentialsAndRolesIsUnautorized_ReturnEmptyString()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.UsersRepository.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(() => new User());
            _mockAuthorizationService.Setup(authService => authService.VerifyHashedPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(() => true);
            _mockAuthorizationService.Setup(authService => authService.GetToken(It.IsAny<User>(), It.IsAny<string>())).Returns(() => "token");
            UsersService userService = new UsersService(_mockUnitOfWork.Object, _mockAuthorizationService.Object, _mockLogger.Object);

            string result = await userService.ValidateAdminCredentials(loginData);

            Assert.IsTrue(string.IsNullOrEmpty(result), "Register procress should fail when user is unautorized");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 1,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task HavingUserServiceInstance_WhenValidatingAdminCredentialsAndRolesIsAutorized_ReturnJwtToken()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.UsersRepository.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(() => new User { Role = Role.Admin });
            _mockAuthorizationService.Setup(authService => authService.VerifyHashedPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(() => true);
            _mockAuthorizationService.Setup(authService => authService.GetToken(It.IsAny<User>(), It.IsAny<string>())).Returns(() => "token");
            UsersService userService = new UsersService(_mockUnitOfWork.Object, _mockAuthorizationService.Object, _mockLogger.Object);

            string result = await userService.ValidateAdminCredentials(loginData);

            Assert.IsTrue(!string.IsNullOrEmpty(result), "Register procress shouldn't fail when user is an admin");

            _mockLogger.Verify(log => log.LogError(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
            _mockLogger.Verify(log => log.LogWarn(It.IsAny<string>()), Times.Never);
            _mockLogger.Verify(log => log.LogInfo(It.IsAny<string>()), Times.Once);
            _mockLogger.Verify(log => log.LogDebug(It.IsAny<string>()), Times.Never);

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 1,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task HavingUserServiceInstance_WhenUpdatingUserAndEmailAlreadyIsRegistered_ReturnFalse()
        {
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.UsersRepository.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(() => new User() { Id = 512 });

            UsersService userService = new UsersService(_mockUnitOfWork.Object, _mockAuthorizationService.Object, _mockLogger.Object);

            bool result = await userService.UpdateUserDetails(1, userData);

            Assert.IsTrue(!result, "Update procress should fail when Email is already registered");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 1,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task HavingUserServiceInstance_WhenUpdatingUserWithSuccess_ReturnTrue()
        {
            int userId = 1;
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.UsersRepository.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(() => new User() { Id = userId });
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.SaveChangesAsync()).ReturnsAsync(() => true);

            UsersService userService = new UsersService(_mockUnitOfWork.Object, _mockAuthorizationService.Object, _mockLogger.Object);

            bool result = await userService.UpdateUserDetails(userId, userData);

            Assert.IsTrue(result, "Update procress shouldn't fail when  it's successful");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task HavingUserServiceInstance_WhenGetUserPublicDataAndUserIsNotFound_ReturnGuestDetails()
        {
            int userId = 1;
            string guestDetails = "Guest";

            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.UsersRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(() => null);

            UsersService userService = new UsersService(_mockUnitOfWork.Object, _mockAuthorizationService.Object, _mockLogger.Object);

            UserPublicData result = await userService.GetUserPublicData(userId);

            Assert.IsNotNull(result, "Resulted data shouldn't be null");
            Assert.AreEqual(result.Email, guestDetails, "User should be a guest");
            Assert.AreEqual(result.FirstName, guestDetails, "User should be a guest");
            Assert.AreEqual(result.LastName, guestDetails, "User should be a guest");

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 1,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task HavingUserServiceInstance_WhenGetUserPublicDataAndUserIsFound_ReturnUserDetails()
        {
            int userId = 1;
            string email = "test@test.test";
            string firstName = "test";
            string lastName = "test";

            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.UsersRepository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(() => new User
            {
                Id = userId,
                Email = email,
                FirstName = firstName,
                LastName = lastName
            });

            UsersService userService = new UsersService(_mockUnitOfWork.Object, _mockAuthorizationService.Object, _mockLogger.Object);

            UserPublicData result = await userService.GetUserPublicData(userId);

            Assert.IsNotNull(result, "Resulted data shouldn't be null");
            Assert.AreEqual(result.Email, email, "User shouldn't be a guest");
            Assert.AreEqual(result.FirstName, firstName, "User shouldn't be a guest");
            Assert.AreEqual(result.LastName, lastName, "User shouldn't be a guest");

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