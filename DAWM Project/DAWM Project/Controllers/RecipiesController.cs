using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Domain;
using RestaurantAPI.Domain.Dtos.RecipeDtos;
using RestaurantAPI.Domain.ServicesAbstractions;

namespace DAWM_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipiesController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        private readonly IDataLogger logger;

        public RecipiesController(IRecipeService recipeService, IDataLogger logger)
        {
            _recipeService = recipeService ?? throw new ArgumentNullException(nameof(recipeService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllIngredients()
        {
            var ingredients = await _recipeService.GetAll();

            return Ok(ingredients);
        }

        [HttpGet]
        [Route("{ingredientId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetIngredientById(int ingredientId)
        {
            var ingredient = await _recipeService.GetById(ingredientId);

            return Ok(ingredient);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddIngredient(CreateOrUpdateRecipe payload)
        {
            bool result = await _recipeService.Create(payload);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("{ingredientId}")]
        public async Task<IActionResult> UpdateIngredient(int ingredientId, CreateOrUpdateRecipe payload)
        {
            bool result = await _recipeService.Update(ingredientId, payload);

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
            bool result = await _recipeService.Delete(ingredientId);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
