using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Domain;
using RestaurantAPI.Domain.Dtos.DishesTypeDtos;
using RestaurantAPI.Domain.Dtos.MenuDtos;
using RestaurantAPI.Domain.ServicesAbstractions;

namespace DAWM_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenusController : ControllerBase
    {
        private readonly IMenusService _menusService;

        private readonly IDataLogger logger;

        public MenusController(IMenusService menusService, IDataLogger logger)
        {
            this._menusService = menusService ?? throw new ArgumentNullException(nameof(menusService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMenus()
        {
            var menus = await _menusService.GetAllMenus();

            return Ok(menus);
        }

        [HttpGet]
        [Route("{menuId}")]
        public async Task<IActionResult> GetMenuById(int menuId)
        {
            var menu = await _menusService.GetMenuById(menuId);

            return Ok(menu);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddMenu(CreateOrUpdateMenu payload)
        {
            bool result = await _menusService.AddMenu(payload);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }


        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("{menuId}")]
        public async Task<IActionResult> UpdateMenu(int menuId, CreateOrUpdateMenu payload)
        {
            bool result = await _menusService.UpateMenu(menuId, payload);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("{menuId}")]
        public async Task<IActionResult> DeleteMenu(int menuId)
        {
            bool result = await _menusService.DeleteMenu(menuId);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("addItem/{menuId}/{recipieId}")]
        public async Task<IActionResult> AddMenuItem(int menuId, int recipieId)
        {
            bool result = await _menusService.AddMenuItem(menuId, recipieId);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("deleteItem/{menuId}/{recipieId}")]
        public async Task<IActionResult> DeleteMenuItem(int menuId, int recipieId)
        {
            bool result = await _menusService.DeleteMenuItem(menuId, recipieId);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
