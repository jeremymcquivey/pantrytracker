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

#pragma warning disable 1591
        public PantryController(IOptions<AppSettings> config,
                                RecipeContext database)
#pragma warning restore 1591
        {
            _db = database;
        }

        /// <summary>
        /// Returns all inventory items belonging to the current user.
        /// </summary>
        [HttpGet]
        public IActionResult GetAll([FromQuery] bool includeZeroValues = false)
        {
            try
            {
                var gId = Guid.Parse(AuthenticatedUser);
                var pantryItems = _db.Transactions.Where(p => p.UserId == gId)
                                                  .Include(p => p.Product)
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

        [HttpPost]
        public async Task<IActionResult> PostTransaction([FromBody] PantryTransaction transaction)
        {
            // TODO: Validate input of transaction i.e. positive/negative quantities, required fields, etc...
            if(transaction == default)
            {
                return BadRequest("Please include a transaction object in the body of the request.");
            }

            transaction.UserId = Guid.Parse(AuthenticatedUser);
            transaction.Product = null;

            _db.Transactions.Add(transaction);
            await _db.SaveChangesAsync();

            return Ok(transaction);
        }
    }
}