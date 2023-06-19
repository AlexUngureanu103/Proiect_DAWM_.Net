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
        /// <param name="payload">Register data</param>
        /// <returns>OkResult if the register process was successful. Otherwise BadRequestResult</returns>
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
        /// <param name="payload">Login data</param>
        /// <returns>OkResult and Jwt Token if the login was successful. Otherwise UnauthorizedResult</returns>
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
        /// <param name="payload">Admin login data</param>
        /// <returns>OkResult and Jwt Token if the login was successful. Otherwise UnauthorizedResult</returns>
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
        /// <param name="paylod">Updated user Data</param>
        /// <returns>OkResult if the update process was successful. Otherwise BadRequestResult</returns>
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
        /// <param name="id">Id of the account to be deleted</param>
        /// <returns>OkResult if the delete process was successful. Otherwise NotFoundResult</returns>
        [HttpDelete("delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            bool response = await _userService.DeleteAccount(id);

            if (response)
            {
                return Ok();
            }

            return NotFound();
        }

        /// <summary>
        /// Get user public data. Authentication required : User, Admin, Guest
        /// </summary>
        /// <returns>OkResult if the get process was successful. Otherwise NotFoundResult</returns>
        [HttpGet("user/data")]
        [Authorize(Roles = "User,Admin,Guest")]
        public async Task<IActionResult> GetUserPublicData()
        {
            ClaimsPrincipal user = User;

            int userId = int.Parse(user.FindFirst("userId").Value);

            var response = await _userService.GetUserPublicData(userId);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }
    }
}
