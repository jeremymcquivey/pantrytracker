using Newtonsoft.Json;
using PantryTracker.Model;
using PantryTracker.Model.Products;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text;

namespace PantryTrackers.Integrations.Walmart
{
    public class WalmartService : IProductSearch
    {
        private Func<string, Task<ProductCode>> _getCodeDelegate;
        private Func<Task<WalmartAuthToken>> _getTokenDelegate;

        private const string oAuthKey = "WalmartAuthToken";
        private const string BaseAPI = "https://developer.api.walmart.com/api-proxy/service/affil/product/v2/";
        private const string AuthBaseURI = "https://walmart-signature-generator.azurewebsites.net/api/";
        private readonly ICacheManager _cache;
        private readonly IHttpClientFactory _requestFactory;
        private readonly ILogger<WalmartService> _logger;

        public string Name => "Walmart API";

        public WalmartService(IHttpClientFactory requestFactory, ICacheManager cache, ILogger<WalmartService> logger)
        {
            _cache = cache;
            _requestFactory = requestFactory;
            _logger = logger;

            _getTokenDelegate = async () => await GetAuthToken();
            _getCodeDelegate = async (string code) => await Search(code);
        }

        public async Task<ProductCode> SearchByCodeAsync(string code)
        {
            return await _cache.GetAsync($"Walmart:{code.ToLower()}", _getCodeDelegate, code, TimeSpan.FromHours(24));
        }

        private async Task<ProductCode> Search(string code)
        {
            var authToken = await _cache.GetAsync(oAuthKey, _getTokenDelegate, TimeSpan.FromMinutes(2));

            using (var request = _requestFactory.CreateClient())
            {
                request.BaseAddress = new Uri(BaseAPI);
                request.DefaultRequestHeaders.Add("WM_SEC.KEY_VERSION", "1");
                request.DefaultRequestHeaders.Add("WM_CONSUMER.ID", authToken.ConsumerId);
                request.DefaultRequestHeaders.Add("WM_CONSUMER.INTIMESTAMP", $"{authToken.TimeStamp}");
                request.DefaultRequestHeaders.Add("WM_SEC.AUTH_SIGNATURE", $"{authToken.Signature}");

                var response = await request.GetAsync($"items?upc={FormatCodeToUPC(code)}");

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
            if(string.IsNullOrEmpty(size))
            {
                return;
            }

            var sizePieces = size?.Split(" ");
            code.Size = sizePieces[0];
            code.Unit = sizePieces.Length > 0 ? string.Join(" ", sizePieces.Skip(1)) : string.Empty;
        }

        private string FormatCodeToUPC(string code)
        {
            return code.Length == 13 && code.StartsWith('0')
                ? code.Substring(1, 12) : code;
        }

        private async Task<WalmartAuthToken> GetAuthToken()
        {
            using (var request = _requestFactory.CreateClient())
            {
                try
                {
                    request.BaseAddress = new Uri(AuthBaseURI);

                    var requestBody = new Dictionary<string, object>
                    {
                        { "ConsumerID", Environment.GetEnvironmentVariable("WalmartAPIConsumerId", EnvironmentVariableTarget.Process) },
                        { "PrivateKeyVersion", int.Parse(Environment.GetEnvironmentVariable("WalmartAPIPrivateKeyVersion", EnvironmentVariableTarget.Process)) }
                    };

                    var response = await request.PostAsync($"HttpExample", new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json"));

                    if (!response.IsSuccessStatusCode)
                    {
                        var responseMessage = await response.Content.ReadAsStringAsync();
                        //TODO: Log into AppInsights event, but don't impede progress.
                        return default;
                    }

                    var rawJson = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<WalmartAuthToken>(rawJson);
                }
                catch (Exception)
                {
                    //TODO: Log into AppInsights event, but don't impede progress.
                    return default;
                }
            }
        }
    }
}
