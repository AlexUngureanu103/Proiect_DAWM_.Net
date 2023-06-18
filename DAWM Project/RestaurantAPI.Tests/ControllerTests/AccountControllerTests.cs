using DAWM_Project.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RestaurantAPI.Domain.Dtos.UserDtos;
using RestaurantAPI.Domain.ServicesAbstractions;
using System.Security.Claims;

namespace RestaurantAPI.Tests.ControllerTests
{
    [TestClass]
    public class AccountControllerTests : LoggerTests
    {
        private Mock<IUsersService> _mockUserService;
        private AccountController accountController;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockUserService = new();
            _mockLogger = new();
            accountController = new(_mockUserService.Object, _mockLogger.Object);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            _mockLogger = null;
            _mockUserService = null;
            accountController = null;
        }

        [TestMethod]
        public void InstantiatingAccountController_WhenIUserServiceIsNull_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new AccountController(null, _mockLogger.Object));
        }

        [TestMethod]
        public void InstantiatingAccountController_WhenIDataLoggerIsNull_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new AccountController(_mockUserService.Object, null));
        }

        [TestMethod]
        public async Task Register_InputIsValid_ReturnsOK()
        {
            var input = new CreateOrUpdateUser { /* create valid user object */ };
            _mockUserService.Setup(service => service.Register(It.IsAny<CreateOrUpdateUser>())).Returns(Task.FromResult(true));

            var result = await accountController.Register(input);

            Assert.IsInstanceOfType(result, typeof(OkResult));

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 1,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task Register_InputIsInvalid_ReturnsBadRequest()
        {
            var input = new CreateOrUpdateUser { /* create invalid user object */ };
            _mockUserService.Setup(service => service.Register(It.IsAny<CreateOrUpdateUser>())).Returns(Task.FromResult(false));

            var result = await accountController.Register(input);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            TestLoggerMethods(
                logErrorCount: 1,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task Test_Login_ReturnUnauthorizedResult()
        {
            _mockUserService.Setup(service => service.ValidateCredentials(It.IsAny<LoginDto>())).ReturnsAsync(() => null);

            var result = await accountController.Login(new LoginDto());

            Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task Test_Login_ReturnOkResult()
        {
            _mockUserService.Setup(service => service.ValidateCredentials(It.IsAny<LoginDto>())).ReturnsAsync("jwtToken");

            var result = await accountController.Login(new LoginDto());

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task Test_LoginAsAdmin_ReturnUnauthorizedResult()
        {
            _mockUserService.Setup(service => service.ValidateAdminCredentials(It.IsAny<LoginDto>())).ReturnsAsync(() => null);

            var result = await accountController.LoginAsAdmin(new LoginDto());

            Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task Test_LoginAsAdmin_ReturnOkResult()
        {
            _mockUserService.Setup(service => service.ValidateAdminCredentials(It.IsAny<LoginDto>())).ReturnsAsync("jwtToken");

            var result = await accountController.LoginAsAdmin(new LoginDto());

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task EditUserDetails_ValidInput_ReturnsOkResult()
        {
            int testId = 1;
            CreateOrUpdateUser testUserPayload = new CreateOrUpdateUser()
            {
                Email = "test@test.com",
                Password = "testpassword",
                FirstName = "Test",
                LastName = "User"
            };

            accountController.ControllerContext = new ControllerContext();
            accountController.ControllerContext.HttpContext = new DefaultHttpContext();
            accountController.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("userId", testId.ToString())
            }));

            _mockUserService.Setup(x => x.UpdateUserDetails(It.IsAny<int>(), It.IsAny<CreateOrUpdateUser>())).ReturnsAsync(true);

            var result = await accountController.EditUserDetails(testUserPayload);

            Assert.IsInstanceOfType(result, typeof(OkResult));

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task EditUserDetails_InvalidInput_ReturnsBadRequestResult()
        {
            int testId = 1;
            CreateOrUpdateUser testUserPayload = new CreateOrUpdateUser()
            {
                Email = "test@test.com",
                Password = "testpassword",
                FirstName = "Test",
                LastName = "User"
            };

            accountController.ControllerContext = new ControllerContext();
            accountController.ControllerContext.HttpContext = new DefaultHttpContext();
            accountController.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("userId", testId.ToString())
            }));

            _mockUserService.Setup(x => x.UpdateUserDetails(It.IsAny<int>(), It.IsAny<CreateOrUpdateUser>())).ReturnsAsync(false);

            var result = await accountController.EditUserDetails(testUserPayload);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task DeleteAccount_InvalidInput_ReturnsBadRequestResult()
        {
            int testId = 1;

            accountController.ControllerContext = new ControllerContext();
            accountController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockUserService.Setup(x => x.DeleteAccount(It.IsAny<int>())).ReturnsAsync(false);

            var result = await accountController.DeleteAccount(testId);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task DeleteAccount_InputIsOk_ReturnsOkResult()
        {
            int testId = 1;

            accountController.ControllerContext = new ControllerContext();
            accountController.ControllerContext.HttpContext = new DefaultHttpContext();

            _mockUserService.Setup(x => x.DeleteAccount(It.IsAny<int>())).ReturnsAsync(true);

            var result = await accountController.DeleteAccount(testId);

            Assert.IsInstanceOfType(result, typeof(OkResult));

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task GetUserPublicData_InvalidInput_ReturnsBadRequestResult()
        {
            int testId = 1;
            CreateOrUpdateUser testUserPayload = new CreateOrUpdateUser()
            {
                Email = "test@test.com",
                Password = "testpassword",
                FirstName = "Test",
                LastName = "User"
            };

            accountController.ControllerContext = new ControllerContext();
            accountController.ControllerContext.HttpContext = new DefaultHttpContext();
            accountController.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("userId", testId.ToString())
            }));

            _mockUserService.Setup(x => x.GetUserPublicData(It.IsAny<int>())).ReturnsAsync(() => null);

            var result = await accountController.GetUserPublicData(testId);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            TestLoggerMethods(
                logErrorCount: 0,
                logErrorExCount: 0,
                logWarnCount: 0,
                logInfoCount: 0,
                logDebugCount: 0
                );
        }

        [TestMethod]
        public async Task GetUserPublicData_InputIsOk_ReturnsOkResult()
        {
            int testId = 1;
            CreateOrUpdateUser testUserPayload = new CreateOrUpdateUser()
            {
                Email = "test@test.com",
                Password = "testpassword",
                FirstName = "Test",
                LastName = "User"
            };

            accountController.ControllerContext = new ControllerContext();
            accountController.ControllerContext.HttpContext = new DefaultHttpContext();
            accountController.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("userId", testId.ToString())
            }));

            _mockUserService.Setup(x => x.GetUserPublicData(It.IsAny<int>())).ReturnsAsync(new UserPublicData());

            var result = await accountController.GetUserPublicData(testId);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

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
