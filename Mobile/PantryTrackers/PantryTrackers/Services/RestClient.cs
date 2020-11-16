using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Extensions.Configuration;
using Xamarin.Essentials;
using PantryTrackers.Common.Security;
using System.Net.Http.Headers;

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

        internal event EventHandler<HttpResponseMessage> OnUnauthorizedResponse; // 401 response
        internal event EventHandler<HttpResponseMessage> OnForbiddenResponse; // 403 response
        internal event EventHandler<HttpResponseMessage> OnNotFoundResponse; // 404 response
        internal event EventHandler<HttpResponseMessage> OnBadRequestErrorOccurred; // other 400-level responses
        internal event EventHandler<HttpResponseMessage> OnServiceUnavailable; // 503 response
        internal event EventHandler<HttpResponseMessage> OnServerErrorOccurred; // other 500-level responses

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
                var response = await _client?.SendAsync(request);
                
                if(response.IsSuccessStatusCode)
                {
                    return response;
                }

                return ProcessError(response);
            }
            catch (HttpRequestException ex)
            {
                if (null != ex.InnerException && ex.InnerException.GetType() == typeof(WebException))
                {
                    return ProcessError(new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent(ex.InnerException.Message)
                    });
                }
                else
                {
                    return ProcessError(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent(ex.Message)
                    });
                }
            }
            catch (Exception ex)
            {
                return ProcessError(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.Message)
                });
            }
        }

        private HttpResponseMessage ProcessError(HttpResponseMessage response)
        {
            // TODO: Log network request failure;

            switch (response.StatusCode)
            {
               case HttpStatusCode.BadRequest:
                    OnBadRequestErrorOccurred?.Invoke(this, response);
                    break;
                case HttpStatusCode.NotFound:
                case HttpStatusCode.Gone:
                    OnNotFoundResponse?.Invoke(this, response);
                    break;
                case HttpStatusCode.ServiceUnavailable:
                    OnServiceUnavailable?.Invoke(this, response);
                    break;
                case HttpStatusCode.Unauthorized:
                    OnUnauthorizedResponse?.Invoke(this, response);
                    break;
                case HttpStatusCode.Forbidden:
                    OnForbiddenResponse?.Invoke(this, response);
                    break;
                case HttpStatusCode.InternalServerError:
                default:
                    OnServerErrorOccurred?.Invoke(this, response);
                    break;
            }

            return response;
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