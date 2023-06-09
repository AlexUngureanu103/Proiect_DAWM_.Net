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
        public async Task<IActionResult> GetAllRecipes()
        {
            var recipes = await _recipeService.GetAll();

            return Ok(recipes);
        }

        [HttpGet]
        [Route("{recipeId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetRecipeById(int recipeId)
        {
            var recipe = await _recipeService.GetById(recipeId);

            return Ok(recipe);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRecipe(CreateOrUpdateRecipe payload)
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
        [Route("{recipeId}")]
        public async Task<IActionResult> UpdateRecipe(int recipeId, CreateOrUpdateRecipe payload)
        {
            bool result = await _recipeService.Update(recipeId, payload);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("{recipeId}")]
        public async Task<IActionResult> DeleteRecipe(int recipeId)
        {
            bool result = await _recipeService.Delete(recipeId);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
