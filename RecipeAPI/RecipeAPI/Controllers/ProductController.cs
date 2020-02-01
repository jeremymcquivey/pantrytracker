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
        private readonly Func<List<Product>> _productDelegate;

        public ProductController(RecipeContext database, KrogerService krogerService, ICacheManager cache)
        {
            _cache = cache;
            _krogerService = krogerService;
            _database = database;

            _productDelegate = () => { return _database.Products.ToList(); };
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
                        var potentialMatch = _database.Products.Where(p => product.Description.Contains(p.Name, StringComparison.CurrentCultureIgnoreCase))
                                                               .OrderByDescending(p => p.Name.Length)
                                                               .ThenByDescending(p => p.OwnerId)
                                                               .FirstOrDefault();
                        if(potentialMatch != default)
                        {
                            product.ProductId = potentialMatch.Id;
                            //product.Description = string.Empty;
                        }

                        //TODO: tie to existing product variety.
                        _database.ProductCodes.Add(product);
                        await _database.SaveChangesAsync();
                        return Ok(product);
                    }
                }

                return Ok(product);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}