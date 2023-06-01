using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Domain;
using RestaurantAPI.Domain.Dtos;

namespace DAWM_Project.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private UsersService UsersService { get; set; }

        private readonly IDataLogger logger;

        public AccountController(UsersService usersService, IDataLogger logger)
        {
            this.UsersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("/register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(CreateOrUpdateUser payload)
        {
            bool result = await UsersService.Register(payload);

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
            bool response = await UsersService.DeleteAccount(id);
            if (response)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
