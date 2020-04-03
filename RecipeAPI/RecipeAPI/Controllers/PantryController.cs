using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using RecipeAPI.Models;
using RecipeAPI.Extensions;
using Microsoft.EntityFrameworkCore;
using PantryTracker.Model.Extensions;
using PantryTracker.Model.Pantry;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using System.Collections.Generic;
using Microsoft.ApplicationInsights.Extensibility;

namespace RecipeAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class PantryController : BaseController
    {
        private readonly RecipeContext _db;
        private readonly TelemetryClient _appInsights;

#pragma warning disable 1591
        public PantryController(IOptions<AppSettings> config,
                                RecipeContext database)
#pragma warning restore 1591
        {
            _db = database;
            _appInsights = new TelemetryClient(TelemetryConfiguration.CreateDefault());
        }

        /// <summary>
        /// Returns all inventory items belonging to the current user.
        /// </summary>
        [HttpGet]
        public IActionResult GetAll([FromQuery] bool includeZeroValues = false)
        {
            try
            {
                _appInsights.TrackEvent("GetAllPantryItems", new Dictionary<string, string> { { "IncludeZeroValues", includeZeroValues ? "true" : "false" } });

                var gId = Guid.Parse(AuthenticatedUser);
                var pantryItems = _db.Transactions.Where(p => p.UserId == gId)
                                                  .Include(p => p.Product)
                                                  .Include(p => p.Variety)
                                                  .CalculateTotals(null)
                                                  .OrderBy(p => p.Product.Name);
                if(!includeZeroValues)
                {
                    return Ok(pantryItems.Where(p => p.Quantity.IsGreaterThan(0, 0.5)));
                }
                return Ok(pantryItems);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Posts a new transaction to the current user's pantry.
        /// </summary>
        [HttpPost]
        [Route("{pantryId}/transaction")]
        public async Task<IActionResult> PostTransaction([FromRoute]string pantryId, [FromBody] PantryTransaction transaction)
        {
            //Note: pantryId is for future use when users can have multiple "pantries", i.e. pantry, kitchen cupboards, fridge, etc...

            // TODO: Validate input of transaction i.e. positive/negative quantities, required fields, etc...
            if(transaction == default)
            {
                return BadRequest("Please include a transaction object in the body of the request.");
            }

            transaction.Id = default;
            transaction.UserId = Guid.Parse(AuthenticatedUser);
            transaction.Product = default;
            transaction.Variety = default;

            _db.Transactions.Add(transaction);
            await _db.SaveChangesAsync();

            return Ok(transaction);
        }
    }
}