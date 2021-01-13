using PantryTrackers.Common.Extensions;
using PantryTrackers.Models.Products;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PantryTrackers.Services
{
    internal class ProductService
    {
        private readonly RestClient _client;

        public ProductService(RestClient client)
        {
            _client = client;
        }

        public async Task<ProductCode> Search(string productText)
        {
            var url = $"v1/ProductCode/{productText}";
            var response = await _client.MakeRequest<object>(new Uri(url, UriKind.Relative), HttpMethod.Get, isSecure: true);
            return await response.GetDeserializedContent<ProductCode>();
        }

        public async Task<ProductCode> Save(ProductCode code)
        {
            if(code.Id > 0)
            {
                //todo: not supported yet.
                return null;
            }

            var url = $"v1/ProductCode";
            var response = await _client.MakeRequest(new Uri(url, UriKind.Relative), HttpMethod.Post, content: code, isSecure: true);
            return await response.GetDeserializedContent<ProductCode>();
        }
    }
}
