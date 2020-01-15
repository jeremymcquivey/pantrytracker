using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Extensions.Options;
using PantryTracker.Model.Recipes;
using PantryTracker.RecipeReader;
using System;
using System.Threading.Tasks;
using System.Linq;
using PantryTracker.ExternalServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RecipeAPI.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using PantryTracker.Model.Products;
using PantryTracker.Model.Extensions;

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

        private readonly IOCRService _ocr;
        private readonly InMemoryProductsDb _products;
        private readonly RecipeContext _db;

#pragma warning disable 1591
        public RecipeController(IOptions<AppSettings> config,
                                RecipeContext database,
                                InMemoryProductsDb products,
								IOCRService ocrService)
#pragma warning restore 1591
        {
            _products = products;
            _db = database;
            _ocr = ocrService;
        }

        /// <summary>
        /// Returns all recipes belonging to the current user.
        /// </summary>
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var recipes = _db.Recipes.Include(r => r.Ingredients)
                                         .Include(r => r.Directions)
                                         .Where(r => r.OwnerId == AuthenticatedUser)
                                         .OrderBy(r => r.Title)
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
        /// Deletes the specified id if it belongs to the current user.
        /// </summary>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute]string id)
        {
            if (!Guid.TryParse(id, out Guid gId))
            {
                return NotFound();
            }

            var recipe = _db.Recipes.SingleOrDefault(x => x.Id == gId && x.OwnerId == AuthenticatedUser);

            if(recipe == default(Recipe))
            {
                return NotFound();
            }

            _db.Recipes.Remove(recipe);
            await _db.SaveChangesAsync();

            return NoContent();
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
            
            var recipe = _db.Recipes.Include(x => x.Ingredients)
                                    .Include(x => x.Directions)
                                    .SingleOrDefault(x => x.Id == gId && x.OwnerId == AuthenticatedUser);

            if(recipe == default(Recipe))
            {
                return NotFound();
            }

            recipe.Ingredients = recipe.Ingredients.OrderBy(i => i.Index);
            recipe.Directions = recipe.Directions.OrderBy(i => i.Index);
            return Ok(recipe);
        }

        /// <summary>
        /// Retrieves a list of products attached to the recipe
        /// </summary>
        [HttpGet]
        [Route("{id}/products")]
        public IActionResult GetProducts([FromRoute]string id)
        {
            if (!Guid.TryParse(id, out Guid gId))
            {
                return NotFound();
            }

            var recipe = _db.Recipes.Include(x => x.Ingredients)
                                    .SingleOrDefault(x => x.Id == gId && x.OwnerId == AuthenticatedUser);

            var userPreferred = _db.UserProductPreferences.Where(x => x.RecipeId == gId);
            var userProducts = _db.Products.Where(product => product.OwnerId == AuthenticatedUser);

            var ignored = new List<Ingredient>();
            var unmatched = new List<Ingredient>();
            var matches = new List<RecipeProduct>();

            foreach (var ingredient in recipe.Ingredients)
            {
                var userMatch = userPreferred.Include(p => p.Product)
                                             .Where(p => ingredient.Name.Contains(p.matchingText, StringComparison.CurrentCultureIgnoreCase))
                                             .OrderBy(p => p.matchingText.Length)
                                             .FirstOrDefault();

                if (userMatch != null)
                {
                    if (userMatch.Product == default)
                    {
                        ignored.Add(ingredient);
                        continue;
                    }

                    matches.Add(new RecipeProduct
                    {
                        Product = userMatch.Product,
                        RecipeId = gId,
                        Unit = ingredient.Unit,
                        Quantity = ingredient.Quantity,
                        Type = IngredientMatchType.UserMatch
                    });

                    continue;
                }

                var potentialMatches = _products.Where(p => ingredient.Name.Contains(p.Name, StringComparison.CurrentCultureIgnoreCase))
                                                .ToList()
                                                .MergeWith(userProducts.Where(p => ingredient.Name.Contains(p.Name, StringComparison.CurrentCultureIgnoreCase)).ToList())
                                                .OrderByDescending(p => p.Name.Length)
                                                .ThenByDescending(p => p.OwnerId);

                if (potentialMatches.Any())
                {
                    matches.Add(new RecipeProduct
                    {
                        Product = potentialMatches.First(),
                        RecipeId = gId,
                        Unit = ingredient.Unit,
                        Quantity = ingredient.Quantity,
                        Type = IngredientMatchType.SystemMatch
                    });

                    continue;
                }

                unmatched.Add(ingredient);
            }

            return Ok(new
            {
                UnmatchedIngredients = unmatched,
                MatchedProducts = matches,
                IgnoredIngredients = ignored
            });
        }

        /// <summary>
        /// Returns the desired recipe.
        /// </summary>
        [HttpPost]
        [Route("preview/image")]
        [Authorize(Roles = "Premium")]
        public async Task<IActionResult> PreviewFromImage([FromBody]string imageText)
        {
            if(string.IsNullOrEmpty(imageText))
            {
                return Ok(new Recipe());
            }

            try
            {
                var ocrText = await _ocr.ImageToText(imageText);
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
            // i.e. similar ingredients/ratios or names.
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
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState.Values.Where(value => value.ValidationState == ModelValidationState.Invalid));
                }

                recipe.OwnerId = AuthenticatedUser;
                recipe.Id = Guid.NewGuid();
                recipe.RawText = Regex.Replace(recipe.RawText, @"\r\n?|\n", Environment.NewLine);

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
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.Where(value => value.ValidationState == ModelValidationState.Invalid));
            }

            if(!Guid.TryParse(id, out Guid gId) || !id.Equals(recipe.Id.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                return BadRequest("");
            }

            var existing = _db.Recipes.AsNoTracking()
                                      .SingleOrDefault(r => r.Id == gId && r.OwnerId == AuthenticatedUser);

            if (existing == default(Recipe))
            {
                return NotFound();
            }

            recipe.RawText = existing.RawText;

            var ingredientIndeces = recipe.Ingredients.Select(i => i.Index);
            _db.RemoveRange(_db.Ingredients.AsNoTracking()
                                           .Where(i => i.RecipeId == gId &&
                                                       !ingredientIndeces.Contains(i.Index)));

            var directionIndeces = recipe.Directions.Select(i => i.Index);
            _db.RemoveRange(_db.Directions.AsNoTracking()
                                          .Where(i => i.RecipeId == gId &&
                                                      !directionIndeces.Contains(i.Index)));

            _db.Update(recipe);
            await _db.SaveChangesAsync();

            return Ok(recipe);
        }
    }
}
