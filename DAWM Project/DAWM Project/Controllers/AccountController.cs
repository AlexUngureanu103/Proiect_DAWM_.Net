using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DAWM_Project.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private UsersService UsersService { get; set; }

        public AccountController(UsersService usersService)
        {
            this.UsersService = usersService;
        }

        [HttpPost("/register")]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return Ok();
        }

        [HttpPost("/login")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return Ok();
        }
    }
}
