using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebViewApp.Xamarin.Core.Constants;
using WebViewApp.Xamarin.Core.Dependency;
using WebViewApp.Xamarin.Core.Enums;
using WebViewApp.Xamarin.Core.Helpers;
using WebViewApp.Xamarin.Core.Ioc;
using WebViewApp.Xamarin.Core.Models;
using WebViewApp.Xamarin.Core.Serialization;

namespace WebViewApp.Xamarin.Core.Services
{
    public interface IApplicationDataService
    {
        Task<TResponse> PostRequest<TResponse, TRequest>(TRequest request) where TResponse : BaseResponse, new()
                                                                                              where TRequest : BaseRequest, new();

        Task<TResponse> GetRequest<TResponse, TRequest>(TRequest request) where TResponse : BaseResponse, new()
                                                                                              where TRequest : BaseRequest, new();
        Task<UpdateDeviceTokenResponse> UpdateDeviceToken(UpdateDeviceTokenRequest request = null);
    }

    public class ApplicationDataService : BaseService, IApplicationDataService
    {
        private readonly IRequestProvider _requestProvider;
        private readonly ISettingsService _settingsService;

        public bool IsLoggedIn
        {
            get
            {
                return !string.IsNullOrEmpty(_settingsService.AuthRefreshToken);
            }
        }

        public ApplicationDataService(IRequestProvider requestProvider, ISettingsService settingsService)
        {
            _requestProvider = requestProvider;
            _settingsService = settingsService;
        }

        public async Task<UpdateDeviceTokenResponse> UpdateDeviceToken(UpdateDeviceTokenRequest request = null)
        {
            UpdateDeviceTokenResponse response = null;

            string uri = UriHelper.CombineUri(GlobalSetting.Instance.BaseGatewayEndpoint, ApiConstants.ApiEndpoint, ApiConstants.UpdateDeviceTokenEndpoint);

            string data = JsonConvert.SerializeObject(request);

            try
            {
                string accessToken = await GetToken();

                response = await _requestProvider.PostAsync<UpdateDeviceTokenResponse>(uri, data, accessToken);
                response.IsSuccessful = true;
            }
            catch (System.Exception ex)
            {
                response = CreateErrorResponse<UpdateDeviceTokenResponse>(ex);
            }

            return response;
        }

        private async Task<string> GetToken()
        {
            var userService = AppDependencyResolver.Resolve<IUserService>();

            var tokenResponse = await userService.GetToken(_settingsService.AuthRefreshToken);

            _settingsService.AuthRefreshToken = tokenResponse.RefreshToken;

            _settingsService.AuthAccessToken = tokenResponse.AccessToken;

            return tokenResponse.AccessToken;
        }

        public async Task<TResponse> PostRequest<TResponse, TRequest>(TRequest request) where TResponse : BaseResponse, new()
                                                                                              where TRequest : BaseRequest, new()
        {
            TResponse response = null;

            string uri = UriHelper.CombineUri(GlobalSetting.Instance.BaseGatewayEndpoint, ApiConstants.ApiEndpoint, request.EndPoint);

            string data = JsonConvert.SerializeObject(request);

            try
            {
                string accessToken = await GetToken();

                response = await _requestProvider.PostAsync<TResponse>(uri, data, accessToken);
                response.IsSuccessful = true;
            }
            catch (System.Exception ex)
            {
                response = CreateErrorResponse<TResponse>(ex);
            }

            return response;
        }

        public async Task<TResponse> GetRequest<TResponse, TRequest>(TRequest request) where TResponse : BaseResponse, new()
                                                                                              where TRequest : BaseRequest, new()
        {
            TResponse response = null;

            string uri = UriHelper.CombineUri(GlobalSetting.Instance.BaseGatewayEndpoint, ApiConstants.ApiEndpoint, request.EndPoint);

            try
            {
                string accessToken = await GetToken();

                response = await _requestProvider.GetAsync<TResponse>(uri, accessToken);
                response.IsSuccessful = true;
            }
            catch (System.Exception ex)
            {
                response = CreateErrorResponse<TResponse>(ex);
            }

            return response;
        }

    }
}
