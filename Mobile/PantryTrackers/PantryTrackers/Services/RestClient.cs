using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using Xamarin.Essentials;
using PantryTrackers.Common.Security;

namespace PantryTrackers.Services
{
    /// <summary>
    /// Base class for RESTful calls to be made via an HTTPClient object.
    /// This class only sends to / receives from services supporting the json data format. 
    /// </summary>
    public sealed class RestClient
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _config;

        /// <summary>
        /// url of the highest common level of the api endpoint
        /// i.e. https://example.domain.com/api
        /// </summary>
        protected Uri BaseApiUrl { get; }

        /// <summary>
        /// Constructor for the RestClient object
        /// </summary>
        public RestClient(HttpClient client, IConfiguration config)
        {
            _config = config;
            _client = client;

            var strUrl = _config.GetSection("Endpoints").GetValue<string>("APIUrl");
            BaseApiUrl = new Uri(strUrl, UriKind.Absolute);
        }

        /// <summary>
        /// Base method for making a request to a REST service.
        /// </summary>
        /// <param name="uri">full path to the api</param>
        /// <param name="type">type of request to make</param>
        /// <param name="requestHeaders">additional http headers to include in the request</param>
        /// <param name="content">body of the response</param>
        /// <returns>result of the request to the service</returns>
        public async Task<HttpResponseMessage> MakeRequest<T>(Uri uri, HttpMethod type, T content = default, bool isSecure = true)
        {
            try
            {
                var json = content != null ? JsonConvert.SerializeObject(content) : null;

                HttpRequestMessage request = new HttpRequestMessage()
                {
                    Method = type,
                    RequestUri = uri.IsAbsoluteUri ? uri : new Uri(BaseApiUrl, uri),
                    Content = json != null ? new StringContent(json, Encoding.UTF8, "application/json") : null,
                };

                var token = await SecureStorage.GetAsync(ClaimKeys.BearerToken);
                if (isSecure && !string.IsNullOrEmpty(token))
                {
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                // if (isLongRunning) { Client.Timeout = System.Threading.Timeout.InfiniteTimeSpan; }
                return await _client?.SendAsync(request);
            }
            catch (HttpRequestException ex)
            {
                if (null != ex.InnerException && ex.InnerException.GetType() == typeof(WebException))
                {
                    throw new Exception("Lost internet connection");
                    //throw new LostNetworkConnectionException();
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent(ex.Message)
                    };
                }
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.Message)
                };
            }
        }

        /*protected async Task<MemoryStream> DownloadFile(Uri uri, HttpMethod type, string filename, string mimeType, Dictionary<string, string> requestHeaders = null, object content = null)
        {
            var response = await MakeRequest(uri, type, requestHeaders, content);

            var memoryStream = new MemoryStream();
            if (response.IsSuccessStatusCode)
            {
                await response.Content.CopyToAsync(memoryStream);
            }

            return memoryStream;
        }*/
    }
}