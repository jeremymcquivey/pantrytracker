using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PantryTracker.Model.Products;
using RecipeAPI.Models;
using System.Threading.Tasks;

namespace RecipeAPI.Controllers
{
    /// <summary>
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductVarietyController : ControllerBase
    {
        private readonly RecipeContext _database;

        /// <summary>
        /// </summary>
        public ProductVarietyController(RecipeContext database)
        {
            _database = database;
        }

        /// <summary>
        /// Adds a new variety of the given product, i.e. salted butter or unsalted butter
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post([FromBody]ProductVariety variety)
        {
            if (variety == default || string.IsNullOrEmpty(variety.Description))
            {
                return BadRequest("A product variety must have a description");
            }

            if (variety.ProductId == default)
            {
                return BadRequest("A product id is required");
            }

            variety.Id = 0;

            _database.Add(variety);
            await _database.SaveChangesAsync();
            return Ok(variety);
        }
    }
}