using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PantryTracker.Model;
using RecipeAPI.ExternalServices;
using RecipeAPI.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeAPI.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class ProductController: BaseController
    {
        private readonly ICacheManager _cache;
        private readonly RecipeContext _database;
        private readonly UPCLookup _productCodes;

        public ProductController(RecipeContext database, UPCLookup productCodes, ICacheManager cache)
        {
            _cache = cache;
            _database = database;
            _productCodes = productCodes;
        }
        
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_database.Products.Where(p => p.OwnerId == null || p.OwnerId == AuthenticatedUser));
        }

        [HttpGet]
        [Route("search/code/{code}")]
        public async Task<IActionResult> GetByUpc([FromRoute]string code)
        {
            try
            {
                var product = await _productCodes.Lookup(code, AuthenticatedUser);

                if(product != default)
                {
                    return Ok(product);
                }

                return NotFound();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}