using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using RecipeAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using PantryTracker.Model.Menu;

namespace RecipeAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class MenuPlanController : BaseController
    {
        private readonly RecipeContext _db;

#pragma warning disable 1591
        public MenuPlanController( RecipeContext database)
#pragma warning restore 1591
        {
            _db = database;
        }

        /// <summary>
        /// Returns all calendar menu entries for current user.
        /// </summary>
        [HttpGet]
        public IActionResult GetAll([FromQuery] string startDate, [FromQuery] string endDate)
        {
            // TODO: Figure out a good limit to the range -- 60 days? This could return a lot of data otherwise.
            if(!DateTime.TryParse(startDate, out DateTime realStartDate))
            {
                realStartDate = DateTime.Today;
            }

            if (!DateTime.TryParse(endDate, out DateTime realEndDate))
            {
                realEndDate = DateTime.Today.AddDays(14);
            }

            try
            {
                var gId = Guid.Parse(AuthenticatedUser);
                var menuEntries = _db.MenuEntries.Include(p => p.Recipe)
                                                 .Where(p => p.OwnerId == gId)
                                                 .Where(p => p.Date <= realEndDate)
                                                 .Where(p => p.Date >= realStartDate)
                                                 .OrderBy(p => p.Date)
                                                 .ToList();

                foreach(var entry in menuEntries)
                {
                    entry.RecipeName = new string(entry.Recipe.Title);
                    entry.Recipe = null;
                }

                return Ok(menuEntries);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddSingle([FromBody] CalendarMenuEntry entry)
        {
            if(entry == default)
            {
                return BadRequest("Please include a CalendarMenuEntry object in the body of the request.");
            }

            if(entry.RecipeId == null || entry.Date == default)
            {
                return BadRequest("Please include recipeId and date properties with your entry object.");
            }

            var gId = Guid.Parse(AuthenticatedUser);
            entry.OwnerId = gId;
            entry.Id = 0;

            try
            {
                var inserted = _db.Add(entry);
                await _db.SaveChangesAsync();
                return Ok(inserted.Entity);
            }
            catch(Exception ex)
            {
                // TODO: Catch exception for dupes here.
                throw;
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> RemoveSingle([FromRoute] int id)
        {
            var gId = Guid.Parse(AuthenticatedUser);
            var existing = _db.MenuEntries.SingleOrDefault(item => item.OwnerId == gId && 
                                                                   item.Id == id);

            if (existing != default)
            {
                _db.Remove(existing);
                await _db.SaveChangesAsync();
            }

            return NoContent();
        }
    }
}
