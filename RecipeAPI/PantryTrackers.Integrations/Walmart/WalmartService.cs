using Newtonsoft.Json;
using PantryTracker.Model;
using PantryTracker.Model.Products;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PantryTrackers.Integrations.Walmart
{
    public class WalmartService : IProductSearch
    {
        private Func<string, Task<ProductCode>> _getCodeDelegate;

        private const string BaseAPI = "https://api.walmartlabs.com/v1/";
        private readonly ICacheManager _cache;
        private readonly IHttpClientFactory _requestFactory;
        private readonly ILogger<WalmartService> _logger;
        private readonly string _clientId;

        public string Name => "Walmart API";

        public WalmartService(IHttpClientFactory requestFactory, ICacheManager cache, ILogger<WalmartService> logger)
        {
            _cache = cache;
            _requestFactory = requestFactory;
            _logger = logger;
            _clientId = Environment.GetEnvironmentVariable("WalmartAPIClientSecret", EnvironmentVariableTarget.Process);

            _getCodeDelegate = async (string code) => await Search(code);
        }

        public async Task<ProductCode> SearchByCodeAsync(string code)
        {
            return await _cache.GetAsync($"Walmart:{code.ToLower()}", _getCodeDelegate, code, TimeSpan.FromHours(24));
        }

        private async Task<ProductCode> Search(string code)
        {
            using (var request = _requestFactory.CreateClient())
            {
                request.BaseAddress = new Uri(BaseAPI);
                request.DefaultRequestHeaders.Add("Accept", "application/json");
                var response = await request.GetAsync($"items?apiKey={_clientId}&upc={FormatCodeToUPC(code)}");

                if (!response.IsSuccessStatusCode)
                {
                    var msg = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Walmart API Error", new { Message = msg }, new { ErrorType = $"{ response.StatusCode }" });
                    return default;
                }

                var rawJson = await response.Content.ReadAsStringAsync();
                var walmartItems = JsonConvert.DeserializeObject<WalmartResponse>(rawJson);

                var singleItem = walmartItems.Items.FirstOrDefault();
                if (singleItem == default)
                {
                    _logger.LogWarning("Code Not Found In Walmart", new { UPC = code });
                    return default;
                }

                var systemItem = new ProductCode
                {
                    Brand = singleItem.BrandName,
                    Code = code,
                    Description = singleItem.Name,
                    Vendor = Name,
                    VendorCode = singleItem.UPC,
                    ProductId = null,
                    Product = null
                };

                ParseSize(systemItem, singleItem.Size);
                return systemItem;
            }
        }

        private void ParseSize(ProductCode code, string size)
        {
            var sizePieces = size.Split(" ");
            code.Size = sizePieces[0];
            code.Unit = sizePieces.Length > 0 ? string.Join(" ", sizePieces.Skip(1)) : string.Empty;
        }

        private string FormatCodeToUPC(string code)
        {
            return code.Length == 13 && code.StartsWith('0')
                ? code.Substring(1, 12) : code;
        }
    }
}
