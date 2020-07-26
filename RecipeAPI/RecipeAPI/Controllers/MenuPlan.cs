using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeAPI.Models;

namespace RecipeAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class MenuPlan : BaseController
    {
        private readonly RecipeContext _db;

#pragma warning disable 1591
        public MenuPlan( RecipeContext database)
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
                var menuEntries = _db.MenuEntries.Where(p => p.OwnerId == gId)
                                                 .Where(p => p.Date <= realEndDate)
                                                 .Where(p => p.Date >= realStartDate)
                                                 .OrderBy(p => p.Date);
                return Ok(menuEntries);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
