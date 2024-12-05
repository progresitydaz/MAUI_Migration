using WebViewApp.Xamarin.Core.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Polly;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebViewApp.Xamarin.Core.Dependency;
using Xamarin.Forms;
using WebViewApp.Xamarin.Core.Models;
using Newtonsoft.Json.Linq;

namespace WebViewApp.Xamarin.Core.Serialization
{
    public interface IRequestProvider
    {
        Task<TResult> GetAsync<TResult>(string uri, string token = "") where TResult : BaseResponse;

        Task<TResult> PostAsync<TResult>(string uri, string data, string token = "", string header = "") where TResult : BaseResponse;

        Task<TResult> PostAsyncAuth<TResult>(string uri, Dictionary<string, string> data, string clientId, string clientSecret) where TResult : BaseResponse;

        Task<TResult> PutAsync<TResult>(string uri, string data, string token = "", string header = "") where TResult : BaseResponse;

        Task<TResult> DeleteAsync<TResult>(string uri, string token = "") where TResult : BaseResponse;
    }

    public class RequestProvider : IRequestProvider
    {
        private readonly JsonSerializerSettings _serializerSettings;

        public RequestProvider()
        {
            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore
            };
            _serializerSettings.Converters.Add(new StringEnumConverter());
        }

        public async Task<TResult> GetAsync<TResult>(string uri, string token = "") where TResult : BaseResponse
        {
            HttpClient httpClient = CreateHttpClient(token);

            var response = await Policy.Handle<WebException>(ex =>
            {
                Debug.WriteLine($"{ex.GetType().Name + " : " + ex.Message}");
                return true;
            })
            .WaitAndRetryAsync
            (
                5,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
             )
             .ExecuteAsync(async () => await httpClient.GetAsync(uri));

            await HandleResponse(response);

            string serialized = await response.Content.ReadAsStringAsync();

            TResult result = await CreateResponse<TResult>(serialized);

            return result;
        }

        public async Task<TResult> PostAsync<TResult>(string uri, string data, string token = "", string header = "") where TResult : BaseResponse
        {
            HttpClient httpClient = CreateHttpClient(token);

            if (!string.IsNullOrEmpty(header))
            {
                AddHeaderParameter(httpClient, header);
            }

            var content = new StringContent(data);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PostAsync(uri, content);

            await HandleResponse(response);

            string serialized = await response.Content.ReadAsStringAsync();

            TResult result = await CreateResponse<TResult>(serialized);

            return result;
        }

        public async Task<TResult> PostAsyncAuth<TResult>(string uri, Dictionary<string, string> data, string clientId, string clientSecret) where TResult : BaseResponse
        {
            HttpClient httpClient = CreateHttpClientForAuth(string.Empty);

            if (!string.IsNullOrWhiteSpace(clientId) && !string.IsNullOrWhiteSpace(clientSecret))
            {
                AddBasicAuthenticationHeader(httpClient, clientId, clientSecret);
            }

            var content = new FormUrlEncodedContent(data);

            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            HttpResponseMessage response = await httpClient.PostAsync(uri, content);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Authorization header has been set, but the server reports that it is missing.
                // It was probably stripped out due to a redirect.

                var finalRequestUri = response.RequestMessage.RequestUri; // contains the final location after following the redirect.

                if (finalRequestUri.AbsolutePath != uri) // detect that a redirect actually did occur.
                {
                    //Reissue the request.The DefaultRequestHeaders configured on the client will be used, so we don't have to set them again.
                    content = new FormUrlEncodedContent(data);
                    response = await httpClient.PostAsync(finalRequestUri, content);
                }
            }

            await HandleResponse(response);

            string serialized = await response.Content.ReadAsStringAsync();

            TResult result = await CreateResponse<TResult>(serialized);

            return result;
        }

        public async Task<TResult> PutAsync<TResult>(string uri, string data, string token = "", string header = "") where TResult : BaseResponse
        {
            HttpClient httpClient = CreateHttpClient(token);

            if (!string.IsNullOrEmpty(header))
            {
                AddHeaderParameter(httpClient, header);
            }

            var content = new StringContent(data);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PutAsync(uri, content);

            await HandleResponse(response);

            string serialized = await response.Content.ReadAsStringAsync();

            TResult result = await CreateResponse<TResult>(serialized);

            return result;
        }

        public async Task<TResult> DeleteAsync<TResult>(string uri, string token = "") where TResult : BaseResponse
        {
            HttpClient httpClient = CreateHttpClient(token);

            HttpResponseMessage response = await httpClient.DeleteAsync(uri);

            await HandleResponse(response);

            string serialized = await response.Content.ReadAsStringAsync();

            TResult result = await CreateResponse<TResult>(serialized);

            return result;
        }

        private async Task<TResult> CreateResponse<TResult>(string serialized) where TResult : BaseResponse
        {
            JObject jObject = (JObject)JsonConvert.DeserializeObject(serialized, _serializerSettings);

            TResult result = await Task.Run(() =>
                jObject["data"].ToObject<TResult>());

            string status = await Task.Run(() =>
                jObject["status"].ToObject<string>());

            result.IsSuccessful = status == "success";

            Debug.WriteLine("_______________________  data _________________________________");
            Debug.WriteLine(serialized);
            Debug.WriteLine("________________________________________________________");

            return result;
        }

        #region HttpClient

        private HttpClient CreateHttpClient(string token = "")
        {
            IHttpClientManager manager = DependencyService.Get<IHttpClientManager>();

            var httpClient = manager != null ? manager.GetHttpClient() : new HttpClient();

            if (httpClient == null)
            {
                httpClient = new HttpClient();
            }

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return httpClient;
        }

        private HttpClient CreateHttpClientForAuth(string token = "")
        {
            IHttpClientManager manager = DependencyService.Get<IHttpClientManager>();

            var httpClient = manager?.GetHttpClient();

            if (httpClient == null)
            {
                httpClient = new HttpClient();
            }

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return httpClient;
        }

        private void AddHeaderParameter(HttpClient httpClient, string parameter)
        {
            if (httpClient == null)
                return;

            if (string.IsNullOrEmpty(parameter))
                return;

            httpClient.DefaultRequestHeaders.Add(parameter, Guid.NewGuid().ToString());
        }

        private void AddBasicAuthenticationHeader(HttpClient httpClient, string clientId, string clientSecret)
        {
            if (httpClient == null)
                return;

            if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
                return;

            httpClient.DefaultRequestHeaders.Authorization = new BasicAuthenticationHeaderValue(clientId, clientSecret);
        }

        private async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.Forbidden ||
                    response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new ServiceAuthenticationException(content);
                }

                throw new HttpRequestExceptionEx(response.StatusCode, content);
            }
        }

        #endregion
    }
}
