using Core.Services;
using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Domain.Dtos;

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
        public async Task<IActionResult> Register(CreateOrUpdateUser payload)
        {
            bool result = await UsersService.Register(payload);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("/login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
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
