using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using RecipeAPI.Models;
using RecipeAPI.Extensions;
using Microsoft.EntityFrameworkCore;
using PantryTracker.Model.Extensions;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using System.Collections.Generic;
using Microsoft.ApplicationInsights.Extensibility;
using RecipeAPI.ExternalServices;
using PantryTracker.Model.Products;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
        private readonly ProductService _products;
        private readonly RecipeContext _database;
        private readonly UPCLookup _productCodes;
        private readonly TelemetryClient _appInsights;

#pragma warning disable 1591
        public ProductController(RecipeContext database, ProductService products, UPCLookup productCodes)
        {
            _products = products;
            _database = database;
            _productCodes = productCodes;
            _appInsights = new TelemetryClient(TelemetryConfiguration.CreateDefault());
        }
#pragma warning restore 1591

        /// <summary>
        /// Admin functionality: Returns a list of all products whose name starts with the provided string.
        /// </summary>
        [HttpGet]
        [Route("search/name/{text}")]
        public IActionResult Get([FromRoute] string text, int limit = 100)
        {
            return Ok(_database.Products.Include(p => p.Varieties)
                                        .Where(p => p.OwnerId == null || p.OwnerId == AuthenticatedUser)
                                        .Where(p => p.Name.Contains(text))
                                        .OrderBy(p => p.Name)
                                        .Take(limit));
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

                if (product != default)
                {
                    return Ok(product);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
        /// Add a new product. If not an admin, this will create an 'individual' product.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody]Product product)
        {
            //TODO: Do a duplicate check.
            if(product == default)
            {
                return BadRequest("Product body object is required");
            }

            if(string.IsNullOrEmpty(product.Name))
            {
                return BadRequest("Name of the product is required");
            }

            product.OwnerId = UserRoles.Contains("Admin") ? null : AuthenticatedUser;
            return Ok(await _products.Add(product));
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

        [HttpGet]
        [Route("{productId}/levelSummary")]
        public IActionResult GetProductSummary([FromRoute]int productId, [FromQuery] string pantryId, [FromQuery] bool includeZeroValues = true)
        {
            try
            {
                _appInsights.TrackEvent("GetPantryProductSummary", new Dictionary<string, string> { { "IncludeZeroValues", includeZeroValues ? "true" : "false" } });

                var gId = Guid.Parse(AuthenticatedUser);
                var pantryItems = _database.Transactions.Where(p => p.UserId == gId)
                                                        .Where(p => p.ProductId == productId)
                                                        .Include(p => p.Variety)
                                                        .Include(p => p.Product)
                                                        .ToList()
                                                        .CalculateProductTotals();

                var otherItems = !includeZeroValues ? pantryItems.Where(p => p.Quantity.IsGreaterThan(0, 0.5)) : pantryItems;

                var groupedItems = otherItems.GroupBy(trans => trans.VarietyId)
                                             .Select(p => new
                                             {
                                                 Header = p.First().Variety?.Description ?? "Unclassified",
                                                 Total = 0,
                                                 Elements = p
                                             });

                return Ok(groupedItems);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}