using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Domain;
using RestaurantAPI.Domain.Dtos;
using RestaurantAPI.Domain.ServicesAbstractions;

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

        [HttpPost("/register")]
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

        [HttpPost("/login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            logger.LogInfo("Loged In");
            return Ok();
        }

        [HttpDelete("/delete")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(int id)
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
