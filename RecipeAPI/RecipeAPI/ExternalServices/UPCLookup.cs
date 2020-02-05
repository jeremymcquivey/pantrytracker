using Microsoft.EntityFrameworkCore;
using PantryTracker.Model;
using PantryTracker.Model.Products;
using PantryTrackers.Integrations.Kroger;
using RecipeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeAPI.ExternalServices
{
    public class UPCLookup
    {
        private readonly Func<Dictionary<Tuple<int, int?>, string[]>> _productDelegate;

        private readonly RecipeContext _database;
        private readonly ICacheManager _cache;
        private readonly IList<IProductSearch> _providers;

        public UPCLookup(RecipeContext database, KrogerService krogerService, ICacheManager cache)
        {
            _database = database;
            _cache = cache;

            _providers = new List<IProductSearch>
            { 
                krogerService
            };

            _productDelegate = () => GetProductBreakdowns(ownerId: null);
        }

        public async Task<ProductCode> Lookup(string code, string ownerId = null)
        {
            var product = _database.ProductCodes
                                   .Include(productCode => productCode.Product)
                                   .OrderByDescending(productCode => productCode.Code == code)
                                   .ThenByDescending(productCode => productCode.Id)
                                   .FirstOrDefault(productCode => (productCode.Code == code || productCode.VendorCode == code) &&
                                                                  (productCode.OwnerId == null || productCode.OwnerId == ownerId));
            if(product != default && product.Vendor != null)
            {
                var provider = _providers.Single(p => p.Name == product.Vendor);
                product = await provider.SearchByCodeAsync(code);
            }
            
            if(product == default)
            {
                foreach (var provider in _providers)
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
                                     })
                                     .Union(varieties)
                                     .ToDictionary(variety => new Tuple<int, int?>(variety.ProductId, variety.Id), p => ((p.Description?.Split(" ", StringSplitOptions.None) ?? new string[0])
                                                                                                                                        .Concat(p.Product.Name.Split(" ", StringSplitOptions.None)))
                                                                                                                                        .ToArray());
        }
    }
}
