using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Extensions.Options;
using PantryTracker.Model.Recipe;
using PantryTracker.RecipeReader;
using System;
using System.Threading.Tasks;
using System.Linq;
using RecipeAPI.Data;
using PantryTracker.ExternalServices;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace RecipeAPI.Controllers
{
    /// <summary>
    /// Manages the creation and updating of all recipes for the current user.
    /// </summary>
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [Authorize]
    //[Authorize(Roles = "Admin")]
    public class RecipeController : BaseController
    {
        private const char EndOfLineDelimiter = '\n';

        private IOCRService _ocr;
        private RecipeContext _db;

#pragma warning disable 1591
        public RecipeController(IOptions<AppSettings> config,
                                RecipeContext database,
								IOCRService ocrService)
#pragma warning restore 1591
        {
            _db = database;
            _ocr = ocrService;
        }

        /// <summary>
        /// Returns all recipes belonging to the current user.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return await Task.Run(() =>
            {
                var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                return Ok();
            });
        }

        /// <summary>
        /// Returns the desired recipe.
        /// </summary>
        [HttpPost]
        [Route("preview/image")]
        public async Task<IActionResult> PreviewFromImage([FromBody]string imageText)
        {
            if(string.IsNullOrEmpty(imageText))
            {
                return Ok(new Recipe());
            }

            try
            {
                var ocrText = _ocr.ImageToText(imageText);
                return await Preview(string.Join(EndOfLineDelimiter, ocrText));
            }
            catch (Exception ex)
            {
                //TODO: Log to app insights.
                throw;
            }
        }

        /// <summary>
        /// Creates a recipe from a raw text block (preview only)
        /// </summary>
        [HttpPost]
		[Route("preview/text")]
        public async Task<IActionResult> Preview([FromBody]string rawText)
        {
            if(string.IsNullOrEmpty(rawText))
            {
                return Ok(new Recipe());
            }

            var parser = new MetadataParser();
            var lines = rawText.Split(EndOfLineDelimiter);
            var output = parser.ExtractRecipe(lines);

            // Returns just the object representation of the recipe. 
            // Future feature: return a list of possible "duplicates" of this recipe.
            // i.e. similar ingredients/ratios.
            return Ok(output);
        }

        /// <summary>
        /// Creates a new recipe within the current user's collection.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]Recipe recipe)
        {
            //TODO: Validate model.
            //TODO: Validate that ingredients don't have overlapping indeces, as this will violate db constraint.
            try
            {
                if(recipe == default(Recipe))
                {
                    return BadRequest("Recipe must be present in the request body.");
                }

                recipe.OwnerId = $@"{Guid.Empty}";
                recipe.Id = Guid.NewGuid();

                foreach (var ingr in recipe.Ingredients)
                {
                    ingr.RecipeId = recipe.Id;
                }

                foreach (var ingr in recipe.Ingredients.Where(i => i.Index == 0))
                {
                    ingr.Index = recipe.Ingredients.Max(p => p.Index) + 1;
                }

                _db.Recipes.Add(recipe);
                await _db.SaveChangesAsync();

                return Ok();
            }
            catch(DocumentClientException ex)
            {
                //TODO: Log to app insights.
                throw;
            }
        }

        /// <summary>
        /// Updates an existing recipe within the current user's collection.
        /// </summary>
        [HttpPatch]
        public IActionResult Update([FromBody]Recipe recipe)
        {
            //TODO: Validate model. -- This needs to have an ID, and it needs to belong to the correct owner.
            return BadRequest("This method is not implemented yet.");
        }
    }
}
