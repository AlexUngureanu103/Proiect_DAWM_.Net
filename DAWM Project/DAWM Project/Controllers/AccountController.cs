using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Domain;
using RestaurantAPI.Domain.Dtos.UserDtos;
using RestaurantAPI.Domain.ServicesAbstractions;
using System.Security.Claims;

namespace DAWM_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUsersService _userService;

        private readonly IDataLogger logger;

        public AccountController(IUsersService usersService, IDataLogger logger)
        {
            this._userService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Register a new user . No authentication required
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(CreateOrUpdateUser payload)
        {
            bool result = await _userService.Register(payload);

            if (result)
            {
                logger.LogInfo("Valid");
                return Ok();
            }

            logger.LogError("Invalid input");
            return BadRequest();
        }

        /// <summary>
        /// Login as a user. No authentication required
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto payload)
        {
            string jwtToken = await _userService.ValidateCredentials(payload);

            if (string.IsNullOrEmpty(jwtToken))
            {
                return Unauthorized();
            }

            return Ok(new { token = jwtToken });
        }

        /// <summary>
        /// Login as an admin. No authentication required
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost("login/adm")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsAdmin(LoginDto payload)
        {
            string jwtToken = await _userService.ValidateAdminCredentials(payload);

            if (string.IsNullOrEmpty(jwtToken))
            {
                return Unauthorized();
            }

            return Ok(new { token = jwtToken });
        }

        /// <summary>
        /// Get user details. Authentication required : User, Admin
        /// </summary>
        /// <param name="paylod"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> EditUserDetails(CreateOrUpdateUser paylod)
        {
            ClaimsPrincipal user = User;

            int userId = int.Parse(user.FindFirst("userId").Value);

            bool result = await _userService.UpdateUserDetails(userId, paylod);
            if (!result)
                return BadRequest();

            return Ok();
        }

        /// <summary>
        /// Delete an account. Authentication required : Admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            bool response = await _userService.DeleteAccount(id);

            if (response)
            {
                return Ok();
            }

            return BadRequest();
        }

        /// <summary>
        /// Get user public data. Authentication required : User, Admin, Guest
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("user/data")]
        [Authorize(Roles = "User,Admin,Guest")]
        public async Task<IActionResult> GetUserPublicData(int id)
        {
            ClaimsPrincipal user = User;

            int userId = int.Parse(user.FindFirst("userId").Value);

            var response = await _userService.GetUserPublicData(userId);

            if (response == null)
            {
                return BadRequest();
            }

            return Ok(response);
        }
    }
}
