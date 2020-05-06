using Microsoft.EntityFrameworkCore;
using PantryTracker.Model;
using PantryTracker.Model.Products;
using RecipeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeAPI.Services
{
    public class ProductService
    {
        private readonly TimeSpan ValidCacheTime = TimeSpan.FromDays(1);
        private readonly Func<Dictionary<Tuple<int, int?>, string[]>>  _productDelegate;
        private readonly RecipeContext _database;
        private readonly ICacheManager _cache;

        public ProductService(ICacheManager cache, RecipeContext database)
        {
            _database = database;
            _cache = cache;

            _productDelegate = () => GetProductBreakdowns(ownerId: null);
        }

        public Product GetById(int id)
        {
            return _database.Products.SingleOrDefault(product => product.Id == id);
        }

        public async Task<Product> Add(Product product)
        {
            product.Id = 0;
            product.DefaultUnit ??= "each";
            product.Varieties = new List<ProductVariety>();
            product.Codes = new List<ProductCode>();

            var newProduct = _database.Products.Add(product);
            await _database.SaveChangesAsync();

            _cache.Add("AllProducts", _productDelegate(), ValidCacheTime);

            return newProduct.Entity;
        }

        public ProductVariety GetVariety(int? id)
        {
            if(id.HasValue)
            {
                return _database.Varieties.SingleOrDefault(variety => variety.Id == id.Value);
            }

            return null;
        }

        public KeyValuePair<Tuple<int, int?>, string[]> MatchProduct(string description)
        {
            var products = _cache.Get("AllProducts", _productDelegate, ValidCacheTime);

            return products.Where(list => list.Value.All(q => description.Contains(q, StringComparison.CurrentCultureIgnoreCase)))
                           .OrderByDescending(p => p.Value.Length)
                           .FirstOrDefault();
        }

        public TextProductMatch GetMatchingProducts(Guid recipeId, string ownerId)
        {
            var recipe = _database.Recipes.Include(x => x.Ingredients)
                                          .SingleOrDefault(x => x.Id == recipeId && x.OwnerId == ownerId);

            var userPreferred = _database.UserProductPreferences.Include(p => p.Product)
                                                          .Include(p => p.Variety)
                                                          .Where(x => x.RecipeId == recipeId)
                                                          .ToList();

            var userProducts = _database.Products.Where(product => product.OwnerId == ownerId)
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
                            Size = ingredient.Size,
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

                var potentialMatch = MatchProduct(ingredient.Name);

                if (potentialMatch.Key != default)
                {
                    matches.Add(new RecipeProduct
                    {
                        Product = GetById(potentialMatch.Key.Item1),
                        Variety = GetVariety(potentialMatch.Key.Item2),
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
                    Size = ingredient.Size,
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

        private Dictionary<Tuple<int, int?>, string[]> GetProductBreakdowns(string ownerId)
        {
            var varieties = _database.Varieties.Include(v => v.Product)
                                               .ToList();

            return _database.Products.Where(p => p.OwnerId == ownerId || p.OwnerId == null)
                                     .Select(p => new ProductVariety
                                     {
                                         Product = p,
                                         ProductId = p.Id,
                                         Description = null,
                                         Id = 0
                                     }).ToList()
                                     .Union(varieties)
                                     .ToDictionary(variety => new Tuple<int, int?>(variety.ProductId, variety.Id), p => ((p.Description?.Split(" ", StringSplitOptions.None) ?? new string[0])
                                                                                                                                        .Concat(p.Product.Name.Split(" ", StringSplitOptions.None)))
                                                                                                                                        .ToArray());
        }
    }
}
