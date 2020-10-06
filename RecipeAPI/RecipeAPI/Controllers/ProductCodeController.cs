using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PantryTracker.Model.Products;
using RecipeAPI.ExternalServices;
using RecipeAPI.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeAPI.Controllers
{
    /// <summary>
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class ProductCodeController : BaseController
    {
        private readonly RecipeContext _database;
        private readonly UPCLookup _productCodes;

        /// <summary>
        /// </summary>
        public ProductCodeController(RecipeContext database, UPCLookup productCodes)
        {
            _database = database;
            _productCodes = productCodes;
        }

        [HttpGet]
        [Route("{code}")]
        public async Task<IActionResult> GetCode([FromRoute]string code)
        {
            var productCode = await _productCodes.Lookup(code, ownerId: AuthenticatedUser);

            if (productCode != default)
            {
                return Ok(productCode);
            }

            return NotFound();
        }

        /// <summary>
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddCode([FromBody]ProductCode productCode)
        {
            // TODO: Verify that if a variety is speicfied that it actually belongs to that product.

            if (productCode == default)
            {
                return BadRequest("ProductCode object is required in request body.");
            }

            if (string.IsNullOrEmpty(productCode.Code) || productCode.Code.Length <= 4 || productCode.Code.Length > 13)
            {
                return BadRequest("4-13 digit code is required.");
            }

            if (productCode.ProductId == null)
            {
                return BadRequest("Please assign a valid product Id");
            }

            if (string.IsNullOrEmpty(productCode.Size) || string.IsNullOrEmpty(productCode.Unit))
            {
                return BadRequest("Size and Unit are both required.");
            }

            productCode.Id = 0;
            productCode.OwnerId = UserRoles.Contains("Admin") ? null : AuthenticatedUser;
            productCode.VendorCode = null;
            productCode.Vendor = null;
            productCode.Product = null;
            productCode.Variety = null;

            if (_database.ProductCodes.Any(p => p.Code == productCode.Code && p.OwnerId == productCode.OwnerId))
            {
                return BadRequest("Duplicate code found.");
            }

            _database.ProductCodes.Add(productCode);
            await _database.SaveChangesAsync();

            return Ok(productCode);
        }
    }
}