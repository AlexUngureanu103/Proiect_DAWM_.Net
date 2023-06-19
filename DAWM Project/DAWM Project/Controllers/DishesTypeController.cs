using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Domain;
using RestaurantAPI.Domain.Dtos.DishesTypeDtos;
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

        /// <summary>
        /// Get all dishes types. No authentication required
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllDishesTypes()
        {
            var dishesTypes = await _dishesTypeService.GetAll();

            return Ok(dishesTypes);
        }

        /// <summary>
        /// Get a dishes type by id. No authentication required
        /// </summary>
        /// <param name="dishesTypeId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{dishesTypeId}")]
        public async Task<IActionResult> GetDishesTypeById(int dishesTypeId)
        {
            var dishesType = await _dishesTypeService.GetById(dishesTypeId);

            if (dishesType == null)
                return NotFound();

            return Ok(dishesType);
        }

        /// <summary>
        /// Add a new dishes type. Authentication required : Admin
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddDishesType(CreateOrUpdateDishesType payload)
        {
            bool result = await _dishesTypeService.Create(payload);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        /// <summary>
        /// Update a dishes type. Authentication required : Admin
        /// </summary>
        /// <param name="dishesTypeId"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("{dishesTypeId}")]
        public async Task<IActionResult> UpdateDishesType(int dishesTypeId, CreateOrUpdateDishesType payload)
        {
            bool result = await _dishesTypeService.Update(dishesTypeId, payload);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        /// <summary>
        /// Delete a dishes type. Authentication required : Admin
        /// </summary>
        /// <param name="dishesTypeId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("{dishesTypeId}")]
        public async Task<IActionResult> DeleteIngredient(int dishesTypeId)
        {
            bool result = await _dishesTypeService.Delete(dishesTypeId);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
