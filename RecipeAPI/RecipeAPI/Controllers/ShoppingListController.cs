using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PantryTracker.Model.Grocery;
using PantryTracker.Model.Products;
using RecipeAPI.Models;
using RecipeAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeAPI.Controllers
{
    /// <summary>
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class ShoppingListController : BaseController
    {
        private readonly RecipeContext _database;
        private readonly ProductService _products;

        public ShoppingListController(RecipeContext database, ProductService products)
        {
            _database = database;
            _products = products;
        }

        /// <summary>
        /// Retrieves a list of products attached to the recipe
        /// </summary>
        [HttpGet]
        [Route("preview/recipe/{id}")]
        public IActionResult GetProducts([FromRoute]string id)
        {
            if (!Guid.TryParse(id, out Guid gId))
            {
                return NotFound();
            }

            return Ok(_products.GetMatchingProducts(gId, AuthenticatedUser));
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetList([FromRoute] string id)
        {
            //TODO: This validation is only temporary, while we don't support multiple pantries.
            if(id != AuthenticatedUser)
            {
                return NotFound();
            }

            //TODO: Combine by product and unit.
            return Ok(_database.GroceryListItems.Where(item => item.PantryId == id && item.Status == ListItemStatus.Active)
                                                .Include(item => item.Variety)
                                                .Include(item => item.Product)
                                                .ToList());
        }

        [HttpPut]
        [Route("{id}/item")]
        public async Task<IActionResult> AddSingle([FromRoute] string id, [FromBody]ListItem item)
        {
            //TODO: This validation is only temporary, while we don't support multiple pantries.
            //Future: This validation will make sure the current user owns the pantry.
            if (id != AuthenticatedUser)
            {
                return NotFound();
            }

            if (item == default)
            {
                return BadRequest("ListItem object must be present in the body.");
            }

            item.PantryId = id;
            item.Variety = null;
            item.Product = null;

            var newEntity = _database.Add(item).Entity;
            await _database.SaveChangesAsync();
            return Ok(newEntity);
        }

        [HttpPut]
        [Route("{id}/items")]
        public async Task<IActionResult> AddBulk([FromRoute] string id, [FromBody]IEnumerable<ListItem> items)
        {
            //TODO: This validation is only temporary, while we don't support multiple pantries.
            //Future: This validation will make sure the current user owns the pantry.
            if (id != AuthenticatedUser)
            {
                return NotFound();
            }

            if (items.Any(item => item == default))
            {
                return BadRequest("ListItem object must be present in the body.");
            }

            foreach(var item in items)
            {
                item.PantryId = id;
                item.Variety = null;
                item.Product = null;
            }

            _database.AddRange(items);
            await _database.SaveChangesAsync();
            return Ok(items);
        }

        [HttpDelete]
        [Route("{id}/item/{itemId}")]
        public async Task<IActionResult> RemoveSingle([FromRoute] string id, [FromRoute] int itemId)
        {
            //TODO: This validation is only temporary, while we don't support multiple pantries.
            //Future: This validation will make sure the current user owns the pantry.
            if (id != AuthenticatedUser)
            {
                return NotFound();
            }

            var existing = _database.GroceryListItems.SingleOrDefault(item => item.PantryId == id && item.Id == itemId);

            if(existing != default)
            {
                _database.Remove(existing);
                await _database.SaveChangesAsync();
            }

            return NoContent();
        }
    }
}