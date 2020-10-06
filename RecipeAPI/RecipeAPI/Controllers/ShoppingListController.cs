using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PantryTracker.Model.Grocery;
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
        private readonly IMapper _mapper;
        private readonly RecipeContext _database;
        private readonly ProductService _products;

        public ShoppingListController(RecipeContext database, ProductService products, IMapper mapper)
        {
            _mapper = mapper;
            _database = database;
            _products = products;
        }

        [HttpGet]
        [Route("{id}/items")]
        public IActionResult GetList([FromRoute] string id)
        {
            //TODO: This validation is only temporary, while we don't support multiple pantries.
            if(id != AuthenticatedUser)
            {
                return NotFound();
            }

            //TODO: Combine by product and unit.
            return Ok(_database.GroceryListItems.Where(item => item.PantryId == id && new[] { ListItemStatus.Active, ListItemStatus.Purchased }.Contains(item.Status))
                                    .Include(item => item.Variety)
                                    .Include(item => item.Product)
                                    .ToList()
                                    .OrderBy(p => p.DisplayName));
        }

        [HttpPost]
        [Route("{id}/items")]
        public async Task<IActionResult> AddBulk([FromRoute] string id, [FromBody]IEnumerable<ListItemViewModel> items)
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

            if (items.Any(item => !item.ProductId.HasValue && string.IsNullOrEmpty(item.FreeformText)))
            {
                return BadRequest("ListItem must either be tied to a produdct id or have a freeform text value.");
            }

            foreach (var item in items)
            {
                item.PantryId = id;
                item.Variety = null;
                item.Product = null;
            }

            _database.AddRange(items.Select(p => _mapper.Map<ListItem>(p)));
            await _database.SaveChangesAsync();
            return Ok(items);
        }

        [HttpPut]
        [Route("{id}/item/{itemId}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromRoute] int itemId, [FromBody] ListItemViewModel item)
        {
            //TODO: This validation is only temporary, while we don't support multiple pantries.
            //Future: This validation will make sure the current user owns the pantry.
            if (id != AuthenticatedUser)
            {
                return NotFound();
            }

            if (item == default)
            {
                return BadRequest("Include a ListItem in your payload.");
            }

            var existing = _database.GroceryListItems.AsNoTracking()
                                                     .SingleOrDefault(p => p.Id == itemId && p.PantryId == id);

            if(existing == default)
            {
                return NotFound();
            }

            if(item.Status == ListItemStatus.Purchased)
            {
                item.PurchaseDate = existing.PurchaseDate ?? DateTime.Now;
            }

            var updatedEntity = _database.Update(_mapper.Map<ListItem>(item)).Entity;
            await _database.SaveChangesAsync();

            return Ok(updatedEntity);
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