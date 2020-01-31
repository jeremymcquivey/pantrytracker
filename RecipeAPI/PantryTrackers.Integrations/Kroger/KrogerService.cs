using Newtonsoft.Json;
using PantryTracker.Model;
using PantryTracker.Model.Products;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PantryTrackers.Integrations.Kroger
{
    public class KrogerService : IProductSearch
    {
        private const string oAuthAPI = "";
        private const string BaseAPI = "https://api.kroger.com/v1/products";

        private readonly IHttpClientFactory _requestFactory;
        private readonly string _clientId;

        public KrogerService(IHttpClientFactory requestFactory)
        {
            _requestFactory = requestFactory;
            _clientId = System.Environment.GetEnvironmentVariable("KrogerAPIClientSecret", System.EnvironmentVariableTarget.Process);
        }

        public async Task<ProductCode> SearchByCodeAsync(string code)
        {
            var krogerUPC = ToKrogerCode(code);
            using (var request = _requestFactory.CreateClient())
            {
                request.BaseAddress = new System.Uri(BaseAPI);
                request.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", GetAuthToken());
                request.DefaultRequestHeaders.Add("Accept", "application/json");
                var response = await request.GetAsync($"?filter.term={krogerUPC}");

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
                    Vendor = "Kroger API",
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
            code.Unit = sizePieces.Length > 0 ? sizePieces[1] : string.Empty;
        }

        private string ToKrogerCode(string upc)
        {
            if (upc.Length == 12)
            {
                upc = $"00{upc.Substring(0, 11)}";
            }

            return upc;
        }

        private string GetAuthToken()
        {
            return "";
        }
    }
}
