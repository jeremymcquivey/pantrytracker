using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.EntityFrameworkCore;
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
        private readonly TelemetryClient _appInsights;
        private readonly ProductService _products;
        private readonly RecipeContext _database;
        private readonly IList<IProductSearch> _providers;

        /// <summary></summary>
        public UPCLookup(RecipeContext database, KrogerService krogerService, WalmartService walmartService, ProductService products)
        {
            _appInsights = new TelemetryClient(TelemetryConfiguration.CreateDefault());
            _products = products;
            _database = database;

            _providers = new List<IProductSearch>
            {
                //krogerService,
                walmartService
            };
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
            var potentialMatch = _products.MatchProduct(code.Description);

            if (potentialMatch.Key != default)
            {
                code.ProductId = potentialMatch.Key.Item1;
                code.VarietyId = potentialMatch.Key.Item2 != 0 ? potentialMatch.Key.Item2 : null;
            }
        }
    }
}
