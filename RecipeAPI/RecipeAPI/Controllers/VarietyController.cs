using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PantryTracker.Model.Products;
using RecipeAPI.Models;
using System.Threading.Tasks;

namespace RecipeAPI.Controllers
{
    /// <summary>
    /// Enables management of product varieties. i.e. creamed corn vs. whole corn
    /// </summary>
    [Route("api/v1")]
    public class VarietyController: BaseController
    {
        private readonly RecipeContext _database;

#pragma warning disable 1591
        public VarietyController(RecipeContext database)
        {
            _database = database;
        }
#pragma warning restore 1591

        /// <summary>
        /// Adds a new variety of the given product, i.e. salted butter or unsalted butter
        /// </summary>
        [HttpPost]
        [Route("product/{productId}/variety")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post([FromRoute]int productId, [FromBody]ProductVariety variety)
        {
            if (variety == default || string.IsNullOrEmpty(variety.Description))
            {
                return BadRequest("A product variety must have a description");
            }

            variety.Id = 0;
            variety.ProductId = productId;

            _database.Add(variety);
            await _database.SaveChangesAsync();
            return Ok(variety);
        }
    }
}
