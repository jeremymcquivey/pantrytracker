using PantryTrackers.Common.Extensions;
using PantryTrackers.Models;
using PantryTrackers.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PantryTrackers.Services
{
    internal class ProductService
    {
        private const int ProductSearchTypeId = 1;
        private const int ProductSearchTypeName = 2;
        private readonly RestClient _client;

        public ProductService(RestClient client)
        {
            _client = client;
        }

        public async Task<Product> ById(int id)
        {
            return (await SearchByText($"{id}", idType: ProductSearchTypeId)).FirstOrDefault();
        }

        public async Task<ProductCode> Search(string productText)
        {
            var url = $"v1/ProductCode/{productText}";
            var response = await _client.MakeRequest<object>(new Uri(url, UriKind.Relative), HttpMethod.Get, isSecure: true);
            return await response.GetDeserializedContent<ProductCode>();
        }

        public async Task<IEnumerable<Product>> SearchByText(string productText, int idType = ProductSearchTypeName)
        {
            var url = $"v1/Product/{productText}?identifierType={idType}";
            var response = await _client.MakeRequest<object>(new Uri(url, UriKind.Relative), HttpMethod.Get, isSecure: true);
            return await response.GetDeserializedContent<IEnumerable<Product>>();
        }

        public async Task<Product> Save(Product product)
        {
            var url = $"v1/Product";
            HttpResponseMessage response;

            if (product.Id > 0)
            {
                response = await _client.MakeRequest(new Uri($"{url}/{product.Id}", UriKind.Relative), HttpMethod.Put, content: product, isSecure: true);
            }
            else
            {
                response = await _client.MakeRequest(new Uri(url, UriKind.Relative), HttpMethod.Post, content: product, isSecure: true);
            }

            return await response.GetDeserializedContent<Product>();
        }

        public async Task<ProductCode> SaveCode(ProductCode code)
        {
            if(code.Id > 0)
            {
                var putResponse = await _client.MakeRequest(new Uri($"v1/ProductCode/{code.Id}", UriKind.Relative), HttpMethod.Put, content: code, isSecure: true);
                return await putResponse.GetDeserializedContent<ProductCode>();
            }

            var url = $"v1/ProductCode";
            var postResponse = await _client.MakeRequest(new Uri(url, UriKind.Relative), HttpMethod.Post, content: code, isSecure: true);
            return await postResponse.GetDeserializedContent<ProductCode>();
        }
    }
}
