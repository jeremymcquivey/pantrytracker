using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PantryTrackers.Integrations.Kroger;
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
        private readonly KrogerService _krogerService;
        private readonly RecipeContext _database;

        public ProductController(RecipeContext database, KrogerService krogerService)
        {
            _krogerService = krogerService;
            _database = database;
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
            var originalSearch = new string(code);
            try
            {
                var product = _database.ProductCodes
                                   .Include(productCode => productCode.Product)
                                   .SingleOrDefault(productCode => (productCode.Code == code || productCode.VendorCode == code) &&
                                                   (productCode.OwnerId == null || productCode.OwnerId == AuthenticatedUser));
                if (product == default)
                {
                    var krogerProduct = await _krogerService.SearchByCodeAsync(code);
                    if(krogerProduct == default)
                    {
                        return NotFound();
                    }

                    //TODO: tie to existing product.
                    _database.ProductCodes.Add(krogerProduct);
                    await _database.SaveChangesAsync();
                    return Ok(krogerProduct);
                }

                return Ok(product);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}