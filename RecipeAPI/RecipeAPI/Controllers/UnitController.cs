using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PantryTracker.Model.Meta;

namespace RecipeAPI.Controllers
{
    /// <summary>
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class UnitController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(new UnitService().AllUnits());
        }
    }
}
