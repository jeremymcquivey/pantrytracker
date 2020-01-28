using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using RecipeAPI.Models;
using RecipeAPI.Extensions;
using Microsoft.EntityFrameworkCore;
using PantryTracker.Model.Extensions;

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
                                                  .CombineUnits()
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
    }
}