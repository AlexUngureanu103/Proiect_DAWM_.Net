using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Domain;
using RestaurantAPI.Domain.Dtos.UserDtos;
using RestaurantAPI.Domain.ServicesAbstractions;
using System.Security.Claims;

namespace DAWM_Project.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IUsersService _userService;

        private readonly IDataLogger logger;

        public AccountController(IUsersService usersService, IDataLogger logger)
        {
            this._userService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

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

        [HttpPut("update")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> EditUserDetails(CreateOrUpdateUser paylod)
        {
            ClaimsPrincipal user = User;

            int userId = int.Parse(user.FindFirst("userId").Value);

            bool result =await _userService.UpdateUserDetails(userId, paylod);
            if (!result)
                return BadRequest();
            
            return Ok();
        }

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
    }
}
