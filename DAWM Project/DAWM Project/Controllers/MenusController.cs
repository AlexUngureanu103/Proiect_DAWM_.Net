﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Domain;
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

        /// <summary>
        /// Get all menus. No authentication required
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllMenus()
        {
            var menus = await _menusService.GetAllMenus();

            return Ok(menus);
        }

        /// <summary>
        /// Get a menu by id. No authentication required
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{menuId}")]
        public async Task<IActionResult> GetMenuById(int menuId)
        {
            var menu = await _menusService.GetMenuById(menuId);

            if (menu == null)
                return NotFound();

            return Ok(menu);
        }

        /// <summary>
        /// Add a new menu. Authentication required : Admin
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Update a menu. Authentication required : Admin
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("{menuId}")]
        public async Task<IActionResult> UpdateMenu(int menuId, CreateOrUpdateMenu payload)
        {
            bool result = await _menusService.UpdateMenu(menuId, payload);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        /// <summary>
        /// Delete a menu. Authentication required : Admin
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Add a recipe to a menu. Authentication required : Admin
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("addItem/{menuId}/{recipeId}")]
        public async Task<IActionResult> AddMenuItem(int menuId, int recipeId)
        {
            bool result = await _menusService.AddMenuItem(menuId, recipeId);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        /// <summary>
        /// Delete a recipe from a menu. Authentication required : Admin
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("deleteItem/{menuId}/{recipeId}")]
        public async Task<IActionResult> DeleteMenuItem(int menuId, int recipeId)
        {
            bool result = await _menusService.DeleteMenuItem(menuId, recipeId);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
