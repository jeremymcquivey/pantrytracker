using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using PantryTracker.Model.Products;
using RecipeAPI.ExternalServices;
using RecipeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeAPI.Controllers
{
    /// <summary>
    /// Gives ability to search for and edit products
    /// </summary>
    [Authorize]
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class ProductController: BaseController
    {
        private readonly RecipeContext _database;
        private readonly UPCLookup _productCodes;

#pragma warning disable 1591
        public ProductController(RecipeContext database, UPCLookup productCodes)
        {
            _database = database;
            _productCodes = productCodes;
        }
#pragma warning restore 1591

        /// <summary>
        /// Admin functionality: Returns a list of all products whose name starts with the provided string.
        /// </summary>
        [HttpGet]
        public IActionResult Get(string searchText, int limit = 100)
        {
            return Ok(_database.Products.Include(p => p.Varieties)
                                        .Where(p => p.OwnerId == null || p.OwnerId == AuthenticatedUser)
                                        .Where(p => p.Name.Contains(searchText))
                                        .OrderBy(p => p.Name)
                                        .Take(limit));
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody]Product product)
        {
            //TODO: Do a duplicate check.
            if(product == default)
            {
                return BadRequest("Product body object is required");
            }

            if(string.IsNullOrEmpty(product.Name))
            {
                return BadRequest("Name of the product is required");
            }

            product.Id = 0;
            product.OwnerId = UserRoles.Contains("Admin") ? null : AuthenticatedUser;
            product.DefaultUnit ??= "each";
            product.Varieties = new List<ProductVariety>();
            product.Codes = new List<ProductCode>();

            _database.Products.Add(product);
            await _database.SaveChangesAsync();

            return Ok(product);
        }

        /// <summary>
        /// Admin functionality: Update the information stored about a given product.
        /// </summary>
        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct([FromRoute]int id, [FromBody]Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.Where(value => value.ValidationState == ModelValidationState.Invalid));
            }

            var existing = _database.Products.AsNoTracking()
                                             .Include(p => p.Codes)
                                             .Include(p => p.Varieties)
                                             .SingleOrDefault(r => r.Id == id && (r.OwnerId == null || r.OwnerId == AuthenticatedUser));

            if (existing == default)
            {
                return NotFound();
            }

            _database.AddRange(product.Varieties.Where(p => p.Id == default));
            _database.RemoveRange(existing.Varieties.Where(p => !product.Varieties.Any(v => v.Id == p.Id)));

            try
            {
                await _database.SaveChangesAsync();
                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Returns a product with the given Id
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok(_database.Products.Include(p => p.Codes)
                                            .ThenInclude(c => c.Variety)
                                        .Include(p => p.Varieties)
                                        .SingleOrDefault(p => p.Id == id && (p.OwnerId == AuthenticatedUser || p.OwnerId == null)));
        }

        /// <summary>
        /// Returns a single product (or null if not found) by product code i.e. UPC, EAN, etc...
        /// </summary>
        [HttpGet]
        [Route("search/code/{code}")]
        public async Task<IActionResult> GetByUpc([FromRoute]string code)
        {
            try
            {
                var product = await _productCodes.Lookup(code, ownerId: AuthenticatedUser);

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

        [HttpPost]
        [Route("{id}/code/{code}")]
        public async Task<IActionResult> AddCode([FromRoute]int id, [FromRoute]string code, [FromBody]ProductCode productCode)
        {
            // TODO: Verify that if a variety is speicfied that it actually belongs to that product.

            if(productCode == default)
            {
                return BadRequest("ProductCode object is required in request body.");
            }

            if(string.IsNullOrEmpty(code) || (code.Length != 12 && code.Length != 13))
            {
                return BadRequest("12 or 13 digit code is required.");
            }

            if(string.IsNullOrEmpty(productCode.Size) || string.IsNullOrEmpty(productCode.Unit))
            {
                return BadRequest("Size and Unit are both required.");
            }

            productCode.Id = 0;
            productCode.ProductId = id;
            productCode.OwnerId = UserRoles.Contains("Admin") ? null : AuthenticatedUser;
            productCode.Code = code;
            productCode.VendorCode = null;
            productCode.Vendor = null;
            productCode.Product = null;

            if (_database.ProductCodes.Any(p => p.Code == code && p.OwnerId == productCode.OwnerId))
            {
                return BadRequest("Duplicate code found.");
            }

            _database.ProductCodes.Add(productCode);
            await _database.SaveChangesAsync();

            return Ok(productCode);
        }
    }
}