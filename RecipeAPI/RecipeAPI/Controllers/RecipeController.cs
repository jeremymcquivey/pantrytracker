﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Extensions.Options;
using PantryTracker.Model.Recipe;
using PantryTracker.RecipeReader;
using System;
using System.Threading.Tasks;
using System.Linq;
using PantryTracker.ExternalServices;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
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
        public IActionResult GetAll()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            
            try
            {
                var recipes = _db.Recipes.Where(r => r.OwnerId == userId)
                                         .ToList();

                foreach(var recipe in recipes)
                {
                    recipe.Ingredients = recipe.Ingredients.OrderBy(i => i.Index);
                    recipe.Directions = recipe.Directions.OrderBy(i => i.Index);
                }

                return Ok(recipes);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Returns an individual recipe by unique Id
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public IActionResult Get([FromRoute]string id)
        {
            if(!Guid.TryParse(id, out Guid gId))
            {
                return NotFound();
            }
            
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var recipe = _db.Recipes.Include(x => x.Ingredients)
                                    .Include(x => x.Directions)
                                    .SingleOrDefault(x => x.Id == gId && x.OwnerId == userId);

            if(recipe == null)
            {
                return NotFound();
            }

            recipe.Ingredients = recipe.Ingredients.OrderBy(i => i.Index);
            recipe.Directions = recipe.Directions.OrderBy(i => i.Index);
            return Ok(recipe);
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
        public async Task<IActionResult> Preview([FromBody]string bodyText)
        {
            if(string.IsNullOrEmpty(bodyText))
            {
                return Ok(new Recipe());
            }

            var parser = new MetadataParser();
            var lines = bodyText.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
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

                recipe.OwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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
            if(recipe == default(Recipe) || !Guid.TryParse(id, out Guid gId) || !id.Equals(recipe.Id.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                return BadRequest("");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var existing = _db.Recipes.AsNoTracking()
                                      .SingleOrDefault(r => r.Id == gId && r.OwnerId == userId);

            if (existing == default(Recipe))
            {
                return NotFound();
            }

            var ingredientIndeces = recipe.Ingredients.Select(i => i.Index);
            foreach (var ingredientToDelete in _db.Ingredients.AsNoTracking()
                                                              .Where(i => i.RecipeId == gId &&
                                                                          !ingredientIndeces.Contains(i.Index)))
            {
                _db.Remove(ingredientToDelete);
            }

            var directionIndeces = recipe.Directions.Select(i => i.Index);
            foreach (var directionToDelete in _db.Directions.AsNoTracking()
                                                            .Where(i => i.RecipeId == gId &&
                                                                        !directionIndeces.Contains(i.Index)))
            {
                _db.Remove(directionToDelete);
            }

            _db.Update(recipe);
            await _db.SaveChangesAsync();

            return Ok(recipe);
        }
    }
}
