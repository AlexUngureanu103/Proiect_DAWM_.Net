using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Domain;
using RestaurantAPI.Domain.Dtos.RecipeDtos;
using RestaurantAPI.Domain.ServicesAbstractions;

namespace DAWM_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipesController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        private readonly IDataLogger logger;

        public RecipesController(IRecipeService recipeService, IDataLogger logger)
        {
            _recipeService = recipeService ?? throw new ArgumentNullException(nameof(recipeService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get all recipes. Authentication required : Admin
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllRecipes()
        {
            var recipes = await _recipeService.GetAll();

            return Ok(recipes);
        }

        /// <summary>
        /// Get a recipe by id. Authentication required : Admin
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{recipeId}")]
        public async Task<IActionResult> GetRecipeById(int recipeId)
        {
            var recipe = await _recipeService.GetById(recipeId);

            if (recipe == null)
                return NotFound();

            return Ok(recipe);
        }

        /// <summary>
        /// Add a new recipe. Authentication required : Admin
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Update a recipe. Authentication required : Admin
        /// </summary>
        /// <param name="recipeId"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Delete a recipe. Authentication required : Admin
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Add an ingredient to a recipe. Authentication required : Admin
        /// </summary>
        /// <param name="recipeId"></param>
        /// <param name="ingredientId"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("addIngredient/{recipeId}/{ingredientId}/{weight}")]
        public async Task<IActionResult> AddRecipeIngredient(int recipeId, int ingredientId, double weight)
        {
            bool result = await _recipeService.AddIngredient(recipeId, ingredientId, weight);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        /// <summary>
        /// Delete an ingredient from a recipe. Authentication required : Admin
        /// </summary>
        /// <param name="recipeId"></param>
        /// <param name="ingredientId"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("deleteIngredient/{recipeId}/{ingredientId}")]
        public async Task<IActionResult> DeleteRecipeIngredient(int recipeId, int ingredientId)
        {
            bool result = await _recipeService.DeleteIngredient(recipeId, ingredientId);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
