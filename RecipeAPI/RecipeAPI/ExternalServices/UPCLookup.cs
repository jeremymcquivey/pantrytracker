using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PantryTracker.Model;
using PantryTracker.Model.Products;
using PantryTrackers.Integrations.Kroger;
using PantryTrackers.Integrations.Walmart;
using RecipeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeAPI.ExternalServices
{
    /// <summary>
    /// Searches list of registered APIs for specified UPC. If no APIs find UPC, default is returned.
    /// </summary>
    public class UPCLookup
    {
        private readonly Func<Dictionary<Tuple<int, int?>, string[]>> _productDelegate;

        private readonly TelemetryClient _appInsights;
        private readonly RecipeContext _database;
        private readonly ICacheManager _cache;
        private readonly IList<IProductSearch> _providers;

        /// <summary></summary>
        public UPCLookup(RecipeContext database, KrogerService krogerService, WalmartService walmartService, ICacheManager cache)
        {
            _appInsights = new TelemetryClient(TelemetryConfiguration.CreateDefault());
            _database = database;
            _cache = cache;

            _providers = new List<IProductSearch>
            {
                //krogerService,
                walmartService
            };

            _productDelegate = () => GetProductBreakdowns(ownerId: null);
        }

        /// <summary>
        /// Looks up a product by UPC from the list of registered APIs.
        /// </summary>
        /// <param name="code">UPC/EAN to find</param>
        /// <param name="preferredProvider">Name of preferred Api i.e. Walmart, Kroger, etc...</param>
        /// <param name="ownerId">Owner of custom products. Leave null for only system products</param>
        public async Task<ProductCode> Lookup(string code, string preferredProvider = null, string ownerId = null)
        {
            var product = _database.ProductCodes
                                   .Include(productCode => productCode.Product)
                                   .OrderByDescending(productCode => productCode.Code == code)
                                   .ThenByDescending(productCode => productCode.Vendor == preferredProvider)
                                   .ThenByDescending(productCode => productCode.Id)
                                   .FirstOrDefault(productCode => (productCode.Code == code || productCode.VendorCode == code) &&
                                                                  (productCode.OwnerId == null || productCode.OwnerId == ownerId));
            if(product != default && product.Vendor != null)
            {
                var provider = _providers.Single(p => p.Name == product.Vendor);
                var newProduct = await provider.SearchByCodeAsync(code);

                if(newProduct != default)
                {
                    product.Brand = newProduct.Brand;
                    product.Description = newProduct.Description;
                    product.Size = newProduct.Size;
                    product.Unit = newProduct.Unit;
                }
            }
            
            if(product == default)
            {
                //TODO: Break out API names into constants to make string comparison more bullet-proof.
                foreach (var provider in _providers.Where(p => preferredProvider == null)
                                                   .OrderBy(p => p.Name.Equals(preferredProvider, StringComparison.InvariantCultureIgnoreCase)))
                {
                    product = await provider.SearchByCodeAsync(code);
                    if (product != default)
                    {
                        AssignProduct(product);

                        _database.ProductCodes.Add(new ProductCode
                        {
                            Code = product.Code,
                            VendorCode = product.VendorCode,
                            ProductId = product.ProductId,
                            VarietyId = product.VarietyId,
                            Vendor = provider.Name
                        });


                        await _database.SaveChangesAsync();
                        return product;
                    }
                }
            }

            return product;
        }

        private void AssignProduct(ProductCode code)
        {
            var products = _cache.Get("AllProducts", _productDelegate, TimeSpan.FromDays(1));

            var potentialMatch = products.Where(list => list.Value.All(q => code.Description.Contains(q, StringComparison.CurrentCultureIgnoreCase)))
                                         .OrderByDescending(p => p.Value.Length)
                                         .FirstOrDefault();

            if (potentialMatch.Key != default)
            {
                code.ProductId = potentialMatch.Key.Item1;
                code.VarietyId = potentialMatch.Key.Item2 != 0 ? potentialMatch.Key.Item2 : null;
            }
        }

        private Dictionary<Tuple<int, int?>, string[]> GetProductBreakdowns(string ownerId)
        {
            var varieties = _database.Varieties.Include(v => v.Product)
                                               .ToList();

            return _database.Products.Where(p => p.OwnerId == ownerId || p.OwnerId == null)
                                     .Select(p => new ProductVariety
                                     {
                                         Product = p,
                                         ProductId = p.Id,
                                         Description = null,
                                         Id = 0
                                     }).ToList()
                                     .Union(varieties)
                                     .ToDictionary(variety => new Tuple<int, int?>(variety.ProductId, variety.Id), p => ((p.Description?.Split(" ", StringSplitOptions.None) ?? new string[0])
                                                                                                                                        .Concat(p.Product.Name.Split(" ", StringSplitOptions.None)))
                                                                                                                                        .ToArray());
        }
    }
}
