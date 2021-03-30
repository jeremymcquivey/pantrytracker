using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using RecipeAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using PantryTracker.Model.Menu;
using System.Collections.Generic;

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
        public ActionResult<IEnumerable<CalendarMenuEntry>> GetAll([FromQuery] string startDate, [FromQuery] string endDate)
        {
            if(!DateTime.TryParse(startDate, out DateTime realStartDate))
            {
                realStartDate = DateTime.Today;
            }

            if (!DateTime.TryParse(endDate, out DateTime realEndDate))
            {
                realEndDate = DateTime.Today.AddDays(14);
            }

            if(realEndDate < realStartDate)
            {
                var temp = new DateTime(realEndDate.Ticks);
                realEndDate = realStartDate;
                realStartDate = temp;
            }

            if ((realEndDate - realStartDate).TotalDays >= 60)
            {
                realEndDate = realStartDate.AddDays(60);
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

                var grouped = menuEntries.GroupBy(p => p.Date.Date)
                                         .ToDictionary(p => p.Key, p => p.AsEnumerable());

                var current = new DateTime(realStartDate.Date.Ticks); 
                while(current <= realEndDate)
                {
                    if(!grouped.ContainsKey(current))
                    {
                        grouped.Add(current, Enumerable.Empty<CalendarMenuEntry>());
                    }
                    current = current.AddDays(1);
                }

                return Ok(grouped.OrderBy(p => p.Key));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<CalendarMenuEntry>> Add([FromBody] CalendarMenuEntry entry)
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
            entry.Date = entry.Date;

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
        public async Task<ActionResult> Remove([FromRoute] int id)
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
