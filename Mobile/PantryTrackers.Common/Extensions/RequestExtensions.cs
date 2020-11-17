using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace PantryTrackers.Common.Extensions
{
    public static class RequestExtensions
    {
        public static async Task<T> GetDeserializedContent<T>(this HttpResponseMessage response, bool useString = false)
        {
            try
            {
                if(!response.IsSuccessStatusCode)
                {
                    return default;
                }

                if(useString)
                {
                    var str = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(str);
                }

                using(var stream = await response.Content.ReadAsStreamAsync())
                using(var reader = new StreamReader(stream))
                using (var json = new JsonTextReader(reader))
                {
                    return new JsonSerializer().Deserialize<T>(json);
                }
            }
            catch(Exception ex)
            {
                // TODO: Log exception here.
                return default;
            }
        }
    }
}
