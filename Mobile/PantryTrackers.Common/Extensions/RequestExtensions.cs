using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

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
                    throw new Exception(await response.Content.ReadAsStringAsync());
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
                await Device.InvokeOnMainThreadAsync(async () =>
                {
                    // TODO: Only show user-friendly errors. System-level or sensitive errors should be logged then genericized.
                    await Application.Current.MainPage.DisplayAlert($"{response.StatusCode} Error", ex.Message, "OK");
                });

                // TODO: Log exception here.
                return default;
            }
        }
    }
}
