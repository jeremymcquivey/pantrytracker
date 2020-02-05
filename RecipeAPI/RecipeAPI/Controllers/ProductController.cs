using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using PantryTracker.Model;
using PantryTracker.Model.Products;
using RecipeAPI.ExternalServices;
using RecipeAPI.Models;
using System;
using System.Collections.Generic;
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
        [Authorize(Roles = "Admin")]
        public IActionResult Get(char startingChar = 'A')
        {
            IEnumerable<Product> realItems;
            var cachedItems = _cache.Get<IList<int>>($"Products:{startingChar}");

            if(cachedItems == default)
            {
                realItems = GetProducts(startingChar);
                _cache.Add($"Products:{startingChar}", realItems.Select(p => p.Id).ToList(), TimeSpan.FromHours(24));
            }
            else
            {
                realItems = _database.Products.Where(p => cachedItems.Contains(p.Id));
            }

            return Ok(realItems);
        }

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

        [HttpPost]
        [Route("{productId}/variety")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SaveNewVariety([FromRoute]int productId, [FromBody]ProductVariety variety)
        {
            if(variety == default || string.IsNullOrEmpty(variety.Description))
            {
                return BadRequest("A variety must have at least a description");
            }

            variety.Id = 0;
            variety.ProductId = productId;

            _database.Add(variety);
            await _database.SaveChangesAsync();
            return Ok(variety);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok(_database.Products.Include(p => p.Codes)
                                            .ThenInclude(c => c.Variety)
                                        .Include(p => p.Varieties)
                                        .SingleOrDefault(p => p.Id == id && (p.OwnerId == AuthenticatedUser || p.OwnerId == null)));
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

        private IEnumerable<Product> GetProducts(char startingLetter)
        {
            return _database.Products.Where(p => p.OwnerId == null || p.OwnerId == AuthenticatedUser)
                                     .Where(p => EF.Functions.Like(p.Name, $"{startingLetter}%"))
                                     .OrderBy(p => p.Name);
        }
    }
}