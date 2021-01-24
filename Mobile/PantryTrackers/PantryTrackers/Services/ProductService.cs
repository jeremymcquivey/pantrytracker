using PantryTrackers.Common.Extensions;
using PantryTrackers.Models;
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
            return await response.GetDeserializedContent<ProductCode>(useString: true);
        }

        public async Task<IEnumerable<Product>> SearchByText(string productText)
        {
            var url = $"v1/Product/{productText}?identifierType=2";
            var response = await _client.MakeRequest<object>(new Uri(url, UriKind.Relative), HttpMethod.Get, isSecure: true);
            return await response.GetDeserializedContent<IEnumerable<Product>>(useString: true);
        }

        public async Task<Product> Save(Product product)
        {
            if (product.Id > 0)
            {
                //todo: not supported yet.
                return null;
            }

            var url = $"v1/Product";
            var response = await _client.MakeRequest(new Uri(url, UriKind.Relative), HttpMethod.Post, content: product, isSecure: true);
            return await response.GetDeserializedContent<Product>();
        }

        public async Task<ProductCode> SaveCode(ProductCode code)
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
