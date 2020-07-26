using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using RecipeAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
