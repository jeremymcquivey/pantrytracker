using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<ProductGroup>> GetSummary(string pantryId)
        {
            var url = $"v1/Pantry/{pantryId}?includeZeroValues=false";
            var response = await _client.MakeRequest<object>(new Uri(url, UriKind.Relative), HttpMethod.Get, isSecure: true);
            return await response.GetDeserializedContent<IEnumerable<ProductGroup>>();
        }

        public async Task<IEnumerable<ProductGroup>> GetProductSummary(int productId)
        {
            var pantryId = await SecureStorage.GetAsync(ClaimKeys.Id);
            var url = $"v1/Pantry/{pantryId}/product/{productId}?includeZeroValues=false";
            var response = await _client.MakeRequest<object>(new Uri(url, UriKind.Relative), HttpMethod.Get, isSecure: true);
            var groups = await response.GetDeserializedContent<IEnumerable<ProductGroup>>();
            return groups;
        }
    }
}
