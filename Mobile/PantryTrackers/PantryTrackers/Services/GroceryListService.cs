using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using PantryTrackers.Common.Extensions;
using PantryTrackers.Models.GroceryList;

namespace PantryTrackers.Services
{
    public class GroceryListService
    {
        private readonly RestClient _client;

        public GroceryListService(RestClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<GroceryListItem>> GetList(string listId)
        {
            var url = $"v1/ShoppingList/{listId}/items";
            var response = await _client.MakeRequest<object>(new Uri(url, UriKind.Relative), HttpMethod.Get, isSecure: true);
            return await response.GetDeserializedContent<IEnumerable<GroceryListItem>>();
        }

        public async Task<GroceryListItem> SaveItem(string listId, GroceryListItem item)
        {
            if (item == null)
            {
                return item;
            }

            if (item.Id == default)
            {
                var url = $"v1/ShoppingList/{listId}/items";
                var response = await _client.MakeRequest(new Uri(url, UriKind.Relative), HttpMethod.Post, content: new List<GroceryListItem> { item }, isSecure: true);
                var responseBody = await response.GetDeserializedContent<IEnumerable<GroceryListItem>>(useString: true);
                return responseBody.First();
            }
            else
            {
                var url = $"v1/ShoppingList/{listId}/item/{item.Id}";
                var response = await _client.MakeRequest(new Uri(url, UriKind.Relative), HttpMethod.Put, content: item, isSecure: true);
                return await response.GetDeserializedContent<GroceryListItem>(useString: true);
            }
        }
    }
}
