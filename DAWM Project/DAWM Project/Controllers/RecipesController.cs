using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Domain;
using RestaurantAPI.Domain.Dtos.RecipeDtos;
using RestaurantAPI.Domain.ServicesAbstractions;

namespace DAWM_Project.Controllers
{
    [ApiController]
    [Route("api/recipes")]
    public class RecipesController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        private readonly IDataLogger logger;

        public RecipesController(IRecipeService recipeService, IDataLogger logger)
        {
            _recipeService = recipeService ?? throw new ArgumentNullException(nameof(recipeService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllIngredients()
        {
            var ingredients = await _recipeService.GetAllRecipes();

            return Ok(ingredients);
        }

        [HttpGet]
        [Route("{ingredientId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetIngredientById(int ingredientId)
        {
            var ingredient = await _recipeService.GetRecipe(ingredientId);

            return Ok(ingredient);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddIngredient(CreateOrUpdateRecipe payload)
        {
            bool result = await _recipeService.AddRecipe(payload);

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
            bool result = await _recipeService.UpdateRecipe(ingredientId, payload);

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
            bool result = await _recipeService.DeleteRecipe(ingredientId);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
