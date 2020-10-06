using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RecipeAPI.Models;
using System.Linq;
using PantryTracker.Model.Products;
using System.Threading.Tasks;
using System;

namespace RecipeAPI.Controllers
{
    /// <summary>
    /// </summary>
    [Authorize]
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class ProductPreferenceController: BaseController
    {
        private RecipeContext _database;

        /// <summary>
        /// </summary>
        /// <param name="database"></param>
        public ProductPreferenceController(RecipeContext database)
        {
            _database = database;
        }

        [HttpPost]
        public async Task<IActionResult> Set([FromBody] UserProductPreference preference)
        {
            if(preference == default)
            {
                return BadRequest("Body must contain a UserProductPreference object");
            }

            var recipe = _database.Recipes.SingleOrDefault(p => p.Id == preference.RecipeId && p.OwnerId == AuthenticatedUser);

            if(recipe == default)
            {
                return NotFound();
            }
                
            try
            {
                var existing = _database.UserProductPreferences.Where(x => x.RecipeId == preference.RecipeId)
                                                               .ToList()
                                                               .Where(x => x.matchingText.Equals(preference.matchingText, StringComparison.CurrentCultureIgnoreCase))
                                                               .FirstOrDefault();
                if (existing != default)
                {
                    _database.UserProductPreferences.Remove(existing);
                }   

                preference.Product = null;
                preference.Variety = null;
                _database.UserProductPreferences.Add(preference);

                await _database.SaveChangesAsync();

                return Ok(preference);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
