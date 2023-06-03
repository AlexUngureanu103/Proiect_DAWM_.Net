using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Domain;
using RestaurantAPI.Domain.Dtos.IngredientDtos;
using RestaurantAPI.Domain.ServicesAbstractions;

namespace DAWM_Project.Controllers
{
    [ApiController]
    [Route("api/ingredients")]
    public class IngredientsController : ControllerBase
    {
        private readonly IIngredientsService _ingredientsService;

        private readonly IDataLogger logger;

        public IngredientsController(IIngredientsService ingredientsService, IDataLogger logger)
        {
            _ingredientsService = ingredientsService ?? throw new ArgumentNullException(nameof(ingredientsService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllIngredients()
        {
            var ingredients = await _ingredientsService.GetAllIngredients();

            return Ok(ingredients);
        }

        [HttpGet]
        [Route("{ingredientId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetIngredientById(int ingredientId)
        {
            var ingredient = await _ingredientsService.GetIngredient(ingredientId);

            return Ok(ingredient);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddIngredient(CreateOrUpdateIngredient payload)
        {
            bool result = await _ingredientsService.AddIngredient(payload);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("{ingredientId}")]
        public async Task<IActionResult> UpdateIngredient(int ingredientId, CreateOrUpdateIngredient payload)
        {
            bool result = await _ingredientsService.UpdateIngredient(ingredientId, payload);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("{ingredientId}")]
        public async Task<IActionResult> DeleteIngredient(int ingredientId)
        {
            bool result = await _ingredientsService.DeleteIngredient(ingredientId);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
