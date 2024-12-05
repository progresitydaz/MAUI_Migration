using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebViewApp.Xamarin.Core.Exceptions;
using WebViewApp.Xamarin.Core.Helpers;
using WebViewApp.Xamarin.Core.Models;
using WebViewApp.Xamarin.Core.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace WebViewApp.Xamarin.Core.Services
{
    public class BaseService
    {
        private readonly JsonSerializerSettings _serializerSettings;

        public BaseService()
        {
            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore
            };
            _serializerSettings.Converters.Add(new StringEnumConverter());
        }

        public T CreateErrorResponse<T>(Exception ex) where T : BaseResponse, new()
        {
            T response = new T()
            {
                IsSuccessful = false,
            };

            try
            {
                JObject jObject = (JObject)JsonConvert.DeserializeObject(ex.Message, _serializerSettings);

                var dataObject = jObject["data"];

                CommonErrorResponse error = dataObject.ToObject<CommonErrorResponse>();

                if (string.IsNullOrEmpty(error.ResultCode))
                {
                    if (dataObject["Result"] != null)
                    {
                        error = dataObject["Result"].ToObject<CommonErrorResponse>();
                    }
                }

                response.Error = error;

                if (ex is TaskCanceledException)
                {
                    response.IsTaskCanceled = true;
                }
                else if (ex is ServiceAuthenticationException)
                {
                    response.IsSessionExpired = true;
                }
            }
            catch (Exception exInner)
            {
                CommonErrorResponse error = CommonErrorResponse.CreateErrorResponseFor(ex);

                string exType = ex.GetType().ToString();

                if (ex is TaskCanceledException)
                {
                    response.IsTaskCanceled = true;
                }
                else if (ex is ServiceAuthenticationException)
                {
                    if (!ex.Message.Contains("SSL"))
                    {
                        response.IsSessionExpired = true;
                    }
                }
                else if (ex is HttpRequestExceptionEx)
                {
                    var httpEx = ex as HttpRequestExceptionEx;

                    response.IsUrlError = httpEx.HttpCode == HttpStatusCode.NotFound;
                }
                else if (ex is HttpRequestException)
                {
                    var httpEx = ex as HttpRequestException;
                }
                else if (ex is TimeoutException)
                {
                    response.IsUrlError = true;
                }
                else if (ex is WebException)
                {
                    response.IsUrlError = true;
                }
                else if (exType == "Java.Net.UnknownHostException")
                {
                    response.IsUrlError = true;
                }
                else if (exType == "Java.IO.IOException")
                {
                    response.IsUrlError = true;
                }

                response.Error = error;

                Debug.WriteLine("exInner-{0}", exInner);
            }

            Debug.WriteLine("ex-{0}", ex);

            LogHelper.LogHandledException("Handled API Web API Error", response.Error.ResultMsgBody);

            return response;
        }

        protected T GetFromCache<T>() where T : BaseResponse
        {
            T data = CacheHelper.Get<T>();

            return data;
        }

        protected void AddToCache<T>(T data) where T : BaseResponse
        {
            CacheHelper.Add<T>(data, TimeSpan.FromMinutes(10));
        }
    }
}
