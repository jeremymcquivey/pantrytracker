using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Newtonsoft.Json;
using PantryTracker.Model;
using PantryTracker.Model.Products;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PantryTrackers.Integrations.Walmart
{
    public class WalmartService : IProductSearch
    {
        private TelemetryClient _appInsights;
        private Func<string, Task<ProductCode>> _getCodeDelegate;

        private const string BaseAPI = "https://api.walmartlabs.com/v1/";
        private readonly ICacheManager _cache;
        private readonly IHttpClientFactory _requestFactory;
        private readonly string _clientId;

        public string Name => "Walmart API";

        public WalmartService(IHttpClientFactory requestFactory, ICacheManager cache)
        {
            _cache = cache;
            _requestFactory = requestFactory;
            _appInsights = new TelemetryClient(TelemetryConfiguration.CreateDefault());
            _clientId = Environment.GetEnvironmentVariable("WalmartAPIClientSecret", EnvironmentVariableTarget.Process);

            _getCodeDelegate = async (string code) => await Search(code);
        }

        public async Task<ProductCode> SearchByCodeAsync(string code)
        {
            return await Search(code);
            //return await _cache.GetAsync($"Walmart:{code.ToLower()}", _getCodeDelegate, code, TimeSpan.FromHours(24));
        }

        private async Task<ProductCode> Search(string code)
        {
            using (var request = _requestFactory.CreateClient())
            {
                request.BaseAddress = new Uri(BaseAPI);
                request.DefaultRequestHeaders.Add("Accept", "application/json");
                var response = await request.GetAsync($"items?apiKey={_clientId}&upc={code}");

                if (!response.IsSuccessStatusCode)
                {
                    var msg = await response.Content.ReadAsStringAsync();
                    _appInsights.TrackEvent("Walmart API Error", new Dictionary<string, string> { { "Message", msg }, { "ErrorType", $"{ response.StatusCode }" } });
                    return default;
                }

                var rawJson = await response.Content.ReadAsStringAsync();
                var walmartItems = JsonConvert.DeserializeObject<WalmartResponse>(rawJson);

                var singleItem = walmartItems.Items.FirstOrDefault();
                if (singleItem == default)
                {
                    _appInsights.TrackEvent("Code Not Found In Walmart", new Dictionary<string, string> { { "UPC", code } });
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
    }
}
