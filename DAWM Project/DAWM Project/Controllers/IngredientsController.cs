using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Domain;
using RestaurantAPI.Domain.Dtos.IngredientDtos;
using RestaurantAPI.Domain.ServicesAbstractions;

namespace DAWM_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IngredientsController : ControllerBase
    {
        private readonly IIngredientsService _ingredientsService;

        private readonly IDataLogger logger;

        public IngredientsController(IIngredientsService ingredientsService, IDataLogger logger)
        {
            _ingredientsService = ingredientsService ?? throw new ArgumentNullException(nameof(ingredientsService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get all ingredients. No authentication required
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllIngredients()
        {
            var ingredients = await _ingredientsService.GetAll();

            return Ok(ingredients);
        }

        /// <summary>
        /// Get an ingredient by id. No authentication required
        /// </summary>
        /// <param name="ingredientId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{ingredientId}")]
        public async Task<IActionResult> GetIngredientById(int ingredientId)
        {
            var ingredient = await _ingredientsService.GetById(ingredientId);

            if (ingredient == null)
                return NotFound();

            return Ok(ingredient);
        }

        /// <summary>
        /// Add a new ingredient. Authentication required : Admin
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddIngredient(CreateOrUpdateIngredient payload)
        {
            bool result = await _ingredientsService.Create(payload);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        /// <summary>
        /// Update an ingredient. Authentication required : Admin
        /// </summary>
        /// <param name="ingredientId"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("{ingredientId}")]
        public async Task<IActionResult> UpdateIngredient(int ingredientId, CreateOrUpdateIngredient payload)
        {
            bool result = await _ingredientsService.Update(ingredientId, payload);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        /// <summary>
        /// Delete an ingredient. Authentication required : Admin
        /// </summary>
        /// <param name="ingredientId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("{ingredientId}")]
        public async Task<IActionResult> DeleteIngredient(int ingredientId)
        {
            bool result = await _ingredientsService.Delete(ingredientId);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
