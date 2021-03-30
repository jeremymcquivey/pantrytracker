using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using RecipeAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using RecipeAPI.ExternalServices;
using PantryTracker.Model.Products;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RecipeAPI.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

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
        private readonly ProductService _products;
        private readonly RecipeContext _database;
        private readonly UPCLookup _productCodes;

#pragma warning disable 1591
        public ProductController(RecipeContext database, ProductService products, UPCLookup productCodes, ILogger<ProductController> logger)
        {
            _products = products;
            _database = database;
            _productCodes = productCodes;
        }
#pragma warning restore 1591

        /// <summary>
        /// Admin functionality: Returns a list of all products whose name starts with the provided string.
        /// </summary>
        [HttpGet]
        [Route("{text}")]
        public async Task<ActionResult<IEnumerable<Product>>> Get([FromRoute] string text, [FromQuery] ProductSearchType identifierType, [FromQuery]int limit = 100)
        {
            switch(identifierType)
            {
                case ProductSearchType.Description:
                    return Ok(_database.Products.Include(p => p.Varieties)
                                                .Where(p => p.OwnerId == null || p.OwnerId == AuthenticatedUser)
                                                .Where(p => p.Name.Contains(text))
                                                .OrderBy(p => p.Name)
                                                .Take(limit));
                case ProductSearchType.RowId:
                default:
                    int.TryParse(text, out int id);
                    return Ok(_database.Products.Include(p => p.Codes)
                                                    .ThenInclude(c => c.Variety)
                                                .Include(p => p.Varieties)
                                                .Where(p => p.Id == id && (p.OwnerId == AuthenticatedUser || p.OwnerId == null)));
            }
        }

        /// <summary>
        /// Add a new product. If not an admin, this will create an 'individual' product.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct([FromBody]Product product)
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

            product.OwnerId = UserRoles.Contains("Admin") ? null : AuthenticatedUser;
            return Ok(await _products.Add(product));
        }
        
        /// <summary>
        /// Admin functionality: Update the information stored about a given product.
        /// </summary>
        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Product>> UpdateProduct([FromRoute]int id, [FromBody]Product product)
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
    }
}