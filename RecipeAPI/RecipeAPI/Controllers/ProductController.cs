using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PantryTracker.Model;
using PantryTracker.Model.Products;
using PantryTrackers.Integrations.Kroger;
using RecipeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeAPI.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class ProductController: BaseController
    {
        private readonly ICacheManager _cache;
        private readonly KrogerService _krogerService;
        private readonly RecipeContext _database;
        private readonly Func<Dictionary<Tuple<int,int?>, string[]>> _productDelegate;

        public ProductController(RecipeContext database, KrogerService krogerService, ICacheManager cache)
        {
            _cache = cache;
            _krogerService = krogerService;
            _database = database;

            _productDelegate = () => GetProductBreakdowns(ownerId: null);
        }
        
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_database.Products.Where(p => p.OwnerId == null || p.OwnerId == AuthenticatedUser));
        }

        [HttpGet]
        [Route("search/code/{code}")]
        public async Task<IActionResult> GetByUpc([FromRoute]string code)
        {
            try
            {
                var product = _database.ProductCodes
                                       .Include(productCode => productCode.Product)
                                       .OrderByDescending(productCode => productCode.Code == code)
                                       .FirstOrDefault(productCode => (productCode.Code == code || productCode.VendorCode == code) && 
                                                                      (productCode.OwnerId == null || productCode.OwnerId == AuthenticatedUser));
                if (product == default)
                {
                    product = await _krogerService.SearchByCodeAsync(code);
                    if(product != default)
                    {
                        AssignProduct(product);

                        //TODO: tie to existing product variety.
                        _database.ProductCodes.Add(product);
                        await _database.SaveChangesAsync();
                        return Ok(product);
                    }
                    return NotFound();
                }

                return Ok(product);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private void AssignProduct(ProductCode code)
        {
            var products = _cache.Get("AllProducts", _productDelegate, TimeSpan.FromDays(1));

            var potentialMatch = products.Where(list => list.Value.All(q => code.Description.Contains(q, StringComparison.CurrentCultureIgnoreCase)))
                                         .OrderByDescending(p => p.Value.Length)
                                         .FirstOrDefault();

            if(potentialMatch.Key != default)
            {
                code.ProductId = potentialMatch.Key.Item1;
                code.VarietyId = potentialMatch.Key.Item2 != 0 ? potentialMatch.Key.Item2 : null;
            }
        }

        private Dictionary<Tuple<int,int?>, string[]> GetProductBreakdowns(string ownerId)
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
                                     })
                                     .Union(varieties)
                                     .ToDictionary(variety => new Tuple<int, int?>(variety.ProductId, variety.Id), p => ((p.Description?.Split(" ", StringSplitOptions.None) ?? new string[0])
                                                                                                                                        .Concat(p.Product.Name.Split(" ", StringSplitOptions.None)))
                                                                                                                                        .ToArray());
        }
    }
}