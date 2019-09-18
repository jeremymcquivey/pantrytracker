using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using PantryTracker.Model.Recipe;
using PantryTracker.RecipeReader;
using System;
using System.Threading.Tasks;
using System.Linq;
using PantryTracker.ExternalServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RecipeAPI.Models;

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
        public RecipeController(RecipeContext database,
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
        public IActionResult GetAll()
        {
            var userId = AuthenticatedUser;
            var recipes = _db.Recipes.Include(r => r.Ingredients)
                                     .Where(r => r.OwnerId == userId)
                                     .ToList();
            
            try
            {
                return Ok(recipes);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Returns the desired recipe.
        /// </summary>
        [HttpPost]
        [Route("preview/image")]
        public IActionResult PreviewFromImage([FromBody]string imageText)
        {
            if(string.IsNullOrEmpty(imageText))
            {
                return Ok(new Recipe());
            }

            try
            {
                // TODO: Log which user is converting the image. Maybe impose a quota for the free tier?
                var ocrText = _ocr.ImageToText(imageText);
                return Preview(string.Join(EndOfLineDelimiter, ocrText));
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
        public IActionResult Preview([FromBody]string rawText)
        {
            if(string.IsNullOrEmpty(rawText))
            {
                return Ok(new Recipe());
            }

            // TODO: Log which user is converting text. This can help us gage usage. This will probably always be on the free tier.
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
            //TODO: Validate that ingredients don't have overlapping indeces, as this will violate db constraint.
            try
            {
                if(recipe == default(Recipe))
                {
                    return BadRequest("Recipe must be present in the request body.");
                }

                recipe.OwnerId = AuthenticatedUser;
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

                return Ok(recipe);
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
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute]string id, [FromBody]Recipe recipe)
        {
            if(recipe == default(Recipe) || !Guid.TryParse(id, out Guid gId) || !id.Equals(recipe.Id))
            {
                return BadRequest("");
            }

            var existing = _db.Recipes.AsNoTracking()
                                      .SingleOrDefault(r => r.Id == gId && r.OwnerId == AuthenticatedUser);

            if(existing == default(Recipe))
            {
                return NotFound();
            }

            _db.Update(recipe);
            await _db.SaveChangesAsync();

            return Ok(recipe);
        }
    }
}
