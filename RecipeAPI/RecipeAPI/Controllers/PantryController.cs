using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using RecipeAPI.Models;
using RecipeAPI.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PantryTracker.Model.Products;

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
        private readonly ILogger<PantryController> _logger;

#pragma warning disable 1591
        public PantryController(ILogger<PantryController> logger,
                                RecipeContext database)
#pragma warning restore 1591
        {
            _db = database;
            _logger = logger;
        }

        /// <summary>
        /// Returns all inventory items belonging to the current user.
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetAll([FromRoute]string id, [FromQuery] bool includeZeroValues = false)
        {
            try
            {
                _logger.LogInformation("Getting pantry level summary", new { id, includeZeroValues });

                var gId = Guid.Parse(AuthenticatedUser);
                var pantryItems = _db.Transactions.Where(p => p.UserId == gId)
                                                  .Include(p => p.Product)
                                                  .Include(p => p.Variety)
                                                  .CalculateTotals(null)
                                                  .OrderBy(p => p.Product?.Name);
                
                var otherItems = !includeZeroValues ? pantryItems.Where(p => p.Quantity > 0) : pantryItems;

                var groupedItems = otherItems.GroupBy(p => p.ProductId)
                                             .Select(p => new
                                             {
                                                 Header = p.First().Product?.Name,
                                                 DisplayMode = p.First().Product?.QuantityDisplayMode ?? ProductDisplayMode.PurchaseQuantity,
                                                 Total =
                                                    (p.First().Product?.QuantityDisplayMode ?? ProductDisplayMode.PurchaseQuantity) == ProductDisplayMode.PurchaseQuantity ?
                                                    $"{p.Sum(q => q.Quantity)} ct" :
                                                    $"{p.Sum(q => q.TotalAmount)} {p.First().Unit}",
                                                 Elements = p
                                             }) ;
                return Ok(groupedItems);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{id}/product/{productId}")]
        public IActionResult GetProductSummary([FromRoute] int productId, [FromQuery] string id, [FromQuery] bool includeZeroValues = true)
        {
            try
            {
                _logger.LogInformation("GetPantryProductSummary", new { IncludeZeroValues = includeZeroValues });

                var gId = Guid.Parse(AuthenticatedUser);
                var pantryItems = _db.Transactions.Where(p => p.UserId == gId)
                                                  .Where(p => p.ProductId == productId)
                                                  .Include(p => p.Variety)
                                                  .Include(p => p.Product)
                                                  .ToList()
                                                  .CalculateProductTotals();

                var otherItems = !includeZeroValues ? pantryItems.Where(p => p.Quantity > 0) : pantryItems;

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