using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Domain;
using RestaurantAPI.Domain.Dtos.DishesTypeDtos;
using RestaurantAPI.Domain.Dtos.IngredientDtos;
using RestaurantAPI.Domain.ServicesAbstractions;

namespace DAWM_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishesTypeController : ControllerBase
    {
        private readonly IDishesTypeService _dishesTypeService;

        private readonly IDataLogger logger;

        public DishesTypeController(IDishesTypeService dishesTypeService, IDataLogger logger)
        {
            this._dishesTypeService = dishesTypeService ?? throw new ArgumentNullException(nameof(dishesTypeService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDishesTypes()
        {
            var dishesTypes = await _dishesTypeService.GetAllDishesTypes();

            return Ok(dishesTypes);
        }

        [HttpGet]
        [Route("{dishesTypeId}")]
        public async Task<IActionResult> GetDishesTypeById(int dishesTypeId)
        {
            var dishesType = await _dishesTypeService.GetDishesType(dishesTypeId);

            return Ok(dishesType);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDishesTypeById(CreateOrUpdateDishesType payload)
        {
            bool result = await _dishesTypeService.AddDishesType(payload);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("{dishesTypeId}")]
        public async Task<IActionResult> UpdateDishesType(int dishesTypeId, CreateOrUpdateDishesType payload)
        {
            bool result = await _dishesTypeService.UpdateDishesType(dishesTypeId, payload);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("{dishesTypeId}")]
        public async Task<IActionResult> DeleteIngredient(int dishesTypeId)
        {
            bool result = await _dishesTypeService.DeleteDishesType(dishesTypeId);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
