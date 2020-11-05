using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace PantryTrackers.Common.Extensions
{
    public static class RequestExtensions
    {
        public static async Task<T> GetDeserializedContent<T>(this HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var strRep = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(strRep);
            }

            return default;
        }
    }
}
