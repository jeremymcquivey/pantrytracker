using Newtonsoft.Json;
using PantryTracker.Model;
using PantryTracker.Model.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PantryTrackers.Integrations.Kroger
{
    public class KrogerService : IProductSearch
    {
        private Func<Task<KrogerAuthToken>> _getTokenDelegate;
        private Func<string, Task<ProductCode>> _getCodeDelegate;

        private const string oAuthKey = "KrogerAuthToken";
        private const string BaseAPI = "https://api.kroger.com/v1/";
        private readonly ICacheManager _cache;
        private readonly IHttpClientFactory _requestFactory;
        private readonly string _clientId;

        public string Name => "Kroger API";

        public KrogerService(IHttpClientFactory requestFactory, ICacheManager cache)
        {
            _cache = cache;
            _requestFactory = requestFactory;
            _clientId = Environment.GetEnvironmentVariable("KrogerAPIClientSecret", EnvironmentVariableTarget.Process);

            _getTokenDelegate = async () => await GetAuthToken();
            _getCodeDelegate = async (string code) => await Search(code);
        }

        public async Task<ProductCode> SearchByCodeAsync(string code)
        {
            return await _cache.GetAsync($"Kroger:{code.ToLower()}", _getCodeDelegate, code, TimeSpan.FromHours(24));
        }

        private async Task<ProductCode> Search(string code)
        {
            var authToken = await _cache.GetAsync(oAuthKey, _getTokenDelegate, TimeSpan.FromMinutes(25));

            if (authToken == default)
            {
                return default;
            }

            var krogerUPC = ToKrogerCode(code);
            using (var request = _requestFactory.CreateClient())
            {
                request.BaseAddress = new Uri(BaseAPI);
                request.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", authToken.access_token);
                request.DefaultRequestHeaders.Add("Accept", "application/json");
                var response = await request.GetAsync($"products?filter.term={krogerUPC}");

                if (!response.IsSuccessStatusCode)
                {
                    return default;
                }

                var rawJson = await response.Content.ReadAsStringAsync();
                var krogerItems = JsonConvert.DeserializeObject<KrogerResponse>(rawJson);

                if (krogerItems.Data.Length == 0)
                {
                    return default;
                }

                var item = krogerItems.Data.First();
                var systemItem = new ProductCode
                {
                    Brand = item.Brand,
                    Code = code,
                    Description = item.Description,
                    Vendor = Name,
                    VendorCode = krogerUPC,
                    ProductId = null,
                    Product = null
                };

                ParseSize(systemItem, item.Items[0].Size);
                return systemItem;
            }
        }

        private void ParseSize(ProductCode code, string size)
        {
            var sizePieces = size.Split(" ");
            code.Size = sizePieces[0];
            code.Unit = sizePieces.Length > 0 ? string.Join(" ", sizePieces.Skip(1)) : string.Empty;
        }

        private string ToKrogerCode(string upc)
        {
            if (upc.Length == 12)
            {
                upc = $"00{upc.Substring(0, 11)}";
            }

            return upc;
        }

        private async Task<KrogerAuthToken> GetAuthToken()
        {
            using (var request = _requestFactory.CreateClient())
            {
                request.BaseAddress = new Uri(BaseAPI);
                request.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("basic", _clientId);
                request.DefaultRequestHeaders.Add("Accept", "application/json");
                
                var requestBody = new Dictionary<string, string>
                {
                    { "grant_type", "client_credentials" },
                    { "scope", "product.compact" }
                };

                var response = await request.PostAsync($"connect/oauth2/token", new FormUrlEncodedContent(requestBody));

                if (!response.IsSuccessStatusCode)
                {
                    var responseMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception(responseMessage);
                }

                var rawJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<KrogerAuthToken>(rawJson);
            }
        }
    }
}
