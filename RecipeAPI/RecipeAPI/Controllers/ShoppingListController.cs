using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PantryTracker.Model.Products;
using RecipeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeAPI.Controllers
{
    /// <summary>
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class ShoppingListController : BaseController
    {
        private readonly RecipeContext _database;
        private readonly ProductService _products;

        public ShoppingListController(RecipeContext database, ProductService products)
        {
            _database = database;
            _products = products;
        }

        /// <summary>
        /// Retrieves a list of products attached to the recipe
        /// </summary>
        [HttpGet]
        [Route("preview/recipe/{id}")]
        public IActionResult GetProducts([FromRoute]string id)
        {
            if (!Guid.TryParse(id, out Guid gId))
            {
                return NotFound();
            }

            return Ok(GetMatchingProducts(gId));
        }

        private TextProductMatch GetMatchingProducts(Guid recipeId)
        {
            var recipe = _database.Recipes.Include(x => x.Ingredients)
                                          .SingleOrDefault(x => x.Id == recipeId && x.OwnerId == AuthenticatedUser);

            var userPreferred = _database.UserProductPreferences.Include(p => p.Product)
                                                          .Include(p => p.Variety)
                                                          .Where(x => x.RecipeId == recipeId)
                                                          .ToList();

            var userProducts = _database.Products.Where(product => product.OwnerId == AuthenticatedUser)
                                                 .ToList();

            var ignored = new List<RecipeProduct>();
            var unmatched = new List<RecipeProduct>();
            var matches = new List<RecipeProduct>();

            foreach (var ingredient in recipe.Ingredients)
            {
                var userMatch = userPreferred.Where(p => ingredient.Name.Contains(p.matchingText, StringComparison.CurrentCultureIgnoreCase))
                                             .OrderBy(p => p.matchingText.Length)
                                             .FirstOrDefault();
                if (userMatch != null)
                {
                    if (userMatch.Product == default(Product))
                    {
                        ignored.Add(new RecipeProduct
                        {
                            MatchType = IngredientMatchType.UserMatch,
                            PlainText = ingredient.Name,
                            QuantityString = ingredient.Quantity,
                            Unit = ingredient.Unit,
                            Size = ingredient.SubQuantity,
                            RecipeId = ingredient.RecipeId,
                        });
                        continue;
                    }

                    matches.Add(new RecipeProduct
                    {
                        Product = userMatch.Product,
                        Variety = userMatch.Variety,
                        RecipeId = recipeId,
                        PlainText = ingredient.Name,
                        Unit = ingredient.Unit,
                        QuantityString = ingredient.Quantity,
                        MatchType = IngredientMatchType.UserMatch
                    });

                    continue;
                }

                var potentialMatch = _products.MatchProduct(ingredient.Name);

                if (potentialMatch.Key != default)
                {
                    matches.Add(new RecipeProduct
                    {
                        Product = _products.GetById(potentialMatch.Key.Item1),
                        Variety = _products.GetVariety(potentialMatch.Key.Item2),
                        RecipeId = recipeId,
                        PlainText = ingredient.Name,
                        Unit = ingredient.Unit,
                        QuantityString = ingredient.Quantity,
                        MatchType = IngredientMatchType.SystemMatch
                    });

                    continue;
                }

                unmatched.Add(new RecipeProduct
                {
                    MatchType = IngredientMatchType.SystemMatch,
                    PlainText = ingredient.Name,
                    QuantityString = ingredient.Quantity,
                    Unit = ingredient.Unit,
                    Size = ingredient.SubQuantity,
                    RecipeId = ingredient.RecipeId,
                });
            }

            return new TextProductMatch
            {
                Matched = matches,
                Unmatched = unmatched,
                Ignored = ignored
            };
        }
    }
}