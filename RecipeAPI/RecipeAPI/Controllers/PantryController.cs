using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using RecipeAPI.Models;
using RecipeAPI.Extensions;
using Microsoft.EntityFrameworkCore;
using PantryTracker.Model.Extensions;
using Microsoft.ApplicationInsights;
using System.Collections.Generic;
using Microsoft.ApplicationInsights.Extensibility;

namespace RecipeAPI.Controllers
{
    /// <summary>
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
        [Route("{id}/levelSummary")]
        public IActionResult GetAll([FromRoute]string id, [FromQuery] bool includeZeroValues = false)
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

                var otherItems = !includeZeroValues ? pantryItems.Where(p => p.Quantity.IsGreaterThanOrEqualTo(0, 0.5)) : pantryItems;

                var groupedItems = otherItems.GroupBy(p => p.ProductId)
                                             .Select(p => new
                                             {
                                                 Header = p.First().Product.Name,
                                                 Total = $"{p.Sum(q => Math.Round(q.Quantity, 2))} {p.First().Unit}",
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