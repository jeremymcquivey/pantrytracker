using System;
using System.Net.Http;
using System.Threading.Tasks;
using PantryTrackers.Common.Extensions;
using PantryTrackers.Common.Security;
using PantryTrackers.Models;
using Xamarin.Essentials;

namespace PantryTrackers.Services
{
    public class PantryService
    {
        private readonly RestClient _client;

        public PantryService(RestClient client)
        {
            _client = client;
        }

        public async Task<PantryTransaction> Save(PantryTransaction transaction)
        {
            var pantryId = await SecureStorage.GetAsync(ClaimKeys.Id);
            var url = $"v1/PantryTransaction/?pantryId={pantryId}";
            var response = await _client.MakeRequest(new Uri(url, UriKind.Relative), HttpMethod.Post, content: transaction, isSecure: true);
            return await response.GetDeserializedContent<PantryTransaction>();
        }
    }
}
