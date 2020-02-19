using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using PantryTracker.Model.Products;
using RecipeAPI.ExternalServices;
using RecipeAPI.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeAPI.Controllers
{
    /// <summary>
    /// Gives ability to search for and edit products
    /// </summary>
    [Authorize]
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class ProductController: BaseController
    {
        private readonly RecipeContext _database;
        private readonly UPCLookup _productCodes;

#pragma warning disable 1591
        public ProductController(RecipeContext database, UPCLookup productCodes)
        {
            _database = database;
            _productCodes = productCodes;
        }
#pragma warning restore 1591

        /// <summary>
        /// Admin functionality: Returns a list of all products whose name starts with the provided string.
        /// </summary>
        [HttpGet]
        public IActionResult Get(string startsWith, int limit = 100)
        {
            return Ok(_database.Products.Include(p => p.Varieties)
                                        .Where(p => p.OwnerId == null || p.OwnerId == AuthenticatedUser)
                                        .Where(p => p.Name.StartsWith(startsWith))
                                        .OrderBy(p => p.Name)
                                        .Take(limit));
        }

        /// <summary>
        /// Admin functionality: Update the information stored about a given product.
        /// </summary>
        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct([FromRoute]int id, [FromBody]Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.Where(value => value.ValidationState == ModelValidationState.Invalid));
            }

            var existing = _database.Products.AsNoTracking()
                                             .Include(p => p.Codes)
                                             .Include(p => p.Varieties)
                                             .SingleOrDefault(r => r.Id == id && (r.OwnerId == null || r.OwnerId == AuthenticatedUser));

            if (existing == default)
            {
                return NotFound();
            }

            _database.AddRange(product.Varieties.Where(p => p.Id == default));
            _database.RemoveRange(existing.Varieties.Where(p => !product.Varieties.Any(v => v.Id == p.Id)));

            try
            {
                await _database.SaveChangesAsync();
                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Returns a product with the given Id
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok(_database.Products.Include(p => p.Codes)
                                            .ThenInclude(c => c.Variety)
                                        .Include(p => p.Varieties)
                                        .SingleOrDefault(p => p.Id == id && (p.OwnerId == AuthenticatedUser || p.OwnerId == null)));
        }

        /// <summary>
        /// Returns a single product (or null if not found) by product code i.e. UPC, EAN, etc...
        /// </summary>
        [HttpGet]
        [Route("search/code/{code}")]
        public async Task<IActionResult> GetByUpc([FromRoute]string code)
        {
            try
            {
                var product = await _productCodes.Lookup(code, ownerId: AuthenticatedUser);

                if(product != default)
                {
                    return Ok(product);
                }

                return NotFound();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}