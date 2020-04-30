using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PantryTracker.Model.Pantry;
using RecipeAPI.Models;

namespace RecipeAPI.Controllers
{
    /// <summary>
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class PantryTransactionController : BaseController
    {
        private readonly RecipeContext _database;

        /// <summary>
        /// </summary>
        public PantryTransactionController(RecipeContext database)
        {
            _database = database;
        }

        /// <summary>
        /// Posts a new transaction to the current user's pantry.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> PostTransaction([FromQuery]string pantryId, [FromBody] PantryTransaction transaction)
        {
            //Note: pantryId is for future use when users can have multiple "pantries", i.e. pantry, kitchen cupboards, fridge, etc...

            // TODO: Validate input of transaction i.e. positive/negative quantities, required fields, etc...
            if (transaction == default)
            {
                return BadRequest("Please include a transaction object in the body of the request.");
            }

            transaction.Id = default;
            transaction.UserId = Guid.Parse(AuthenticatedUser);
            transaction.Product = default;
            transaction.Variety = default;

            if (transaction.TransactionType == PantryTransactionType.Usage)
            {
                transaction.Quantity = -1 * Math.Abs(transaction.Quantity);
            }

            _database.Transactions.Add(transaction);
            await _database.SaveChangesAsync();

            return Ok(transaction);
        }
    }
}