using Microsoft.AspNetCore.Mvc;
using RecipeAPI.Model;
using System.Threading.Tasks;

namespace RecipeAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class RecipeController : Controller
    {
        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok();
        }

        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            return Ok();
        }

        [HttpPost]
        //[Authorize(Roles = "AddRecipe")]
        public async Task<IActionResult> Create(Recipe recipe)
        {
            return Ok();
        }

        //[Authorize]
        [HttpPatch]
        public async Task<IActionResult> Update(Recipe recipe)
        {
            return Ok();
        }
    }
}