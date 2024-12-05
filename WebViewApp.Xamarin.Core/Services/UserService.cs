using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebViewApp.Xamarin.Core.Constants;
using WebViewApp.Xamarin.Core.Exceptions;
using WebViewApp.Xamarin.Core.Helpers;
using WebViewApp.Xamarin.Core.Models;
using WebViewApp.Xamarin.Core.Serialization;

namespace WebViewApp.Xamarin.Core.Services
{
    public interface IUserService
    {
        Task<AuthenticationResponse> Authenticate(AuthenticationRequest request);
        Task<SmsVerificationResponse> VerifySmsToken(SmsVerificationRequest request);
        Task<CallSmsTokenResponse> CallSmsToken(CallSmsTokenRequest request);
        Task<GetAuthTokenResponse> GetToken(string refreshToken);
        Task<LogoutResponse> Logout(LogoutRequest logoutRequest);
    }

    public class UserService : BaseService, IUserService
    {
        private readonly IRequestProvider _requestProvider;
        private readonly ISettingsService _settingsService;

        public UserService(IRequestProvider requestProvider, ISettingsService settingsService)
        {
            _requestProvider = requestProvider;
            _settingsService = settingsService;
        }

        public async Task<AuthenticationResponse> Authenticate(AuthenticationRequest request)
        {
            AuthenticationResponse response = null;

            var uri = UriHelper.CombineUri(GlobalSetting.Instance.BaseGatewayEndpoint, ApiConstants.AuthenticateEndpoint);

            var data = new Dictionary<string, string>
            {
                { "grant_type", "password" },
                { "scope", "read+trust+write" },
                { "username", request.UserName },
                { "password", request.Password },
                { "deviceToken", request.DeviceToken },
                { "appVersion", request.AppVersion },
                { "devicePlatform", request.DevicePlatform }
            };

            try
            {
                response = await _requestProvider.PostAsyncAuth<AuthenticationResponse>(uri, data, ApiConstants.ClientId, ApiConstants.ClientSecret);
                response.IsSuccessful = response.IsLoginOk;
            }
            catch (System.Exception ex)
            {
                response = CreateErrorResponse<AuthenticationResponse>(ex);
            }

            return response;
        }

        public async Task<CallSmsTokenResponse> CallSmsToken(CallSmsTokenRequest request)
        {
            CallSmsTokenResponse response = null;

            var uri = UriHelper.CombineUri(GlobalSetting.Instance.BaseGatewayEndpoint, ApiConstants.CallSmsTokenEndPoint);

            try
            {
                //string data = JsonConvert.SerializeObject(request);

                var data = new Dictionary<string, string>
                {
                    { "username", request.UserName },
                    { "password", request.Password },
                    { "deviceToken", request.DeviceToken },
                    { "appVersion", request.AppVersion },
                    { "devicePlatform", request.DevicePlatform }
                };

                response = await _requestProvider.PostAsyncAuth<CallSmsTokenResponse>(uri, data, ApiConstants.ClientId, ApiConstants.ClientSecret);

                response = new CallSmsTokenResponse()
                {
                    IsSuccessful = true,
                };

            }
            catch (System.Exception ex)
            {
                response = CreateErrorResponse<CallSmsTokenResponse>(ex);
            }

            return response;
        }

        public async Task<SmsVerificationResponse> VerifySmsToken(SmsVerificationRequest request)
        {
            SmsVerificationResponse response = null;

            try
            {
                var uri = UriHelper.CombineUri(GlobalSetting.Instance.BaseGatewayEndpoint, ApiConstants.SmsTokenEndpoint);

                var data = new Dictionary<string, string>
                {
                    { "grant_type", "password" },
                    { "username", request.UserName },
                    { "password", request.Password },
                    { "tokenpart1", request.Token1 },
                    { "tokenpart2", request.Token2 },
                    { "deviceToken", request.DeviceToken },
                    { "devicePlatform", request.DevicePlatform }
                };

                response = await _requestProvider.PostAsyncAuth<SmsVerificationResponse>(uri, data, ApiConstants.ClientId, ApiConstants.ClientSecret);

                response.IsSuccessful = !string.IsNullOrEmpty(response.AccessToken);
            }
            catch (Exception ex)
            {
                response = CreateErrorResponse<SmsVerificationResponse>(ex);
            }

            return response;
        }

        public async Task<GetAuthTokenResponse> GetToken(string refreshToken)
        {
            GetAuthTokenResponse response = null;

            try
            {
                var uri = UriHelper.CombineUri(GlobalSetting.Instance.BaseGatewayEndpoint, ApiConstants.TokenEndpoint);

                var data = new Dictionary<string, string>
                {
                    { "grant_type", "refresh_token" },
                    { "refresh_token", refreshToken },
                };

                response = await _requestProvider.PostAsyncAuth<GetAuthTokenResponse>(uri, data, ApiConstants.ClientId, ApiConstants.ClientSecret);

                response.IsSuccessful = !string.IsNullOrEmpty(response.AccessToken);
            }
            catch (Exception ex)
            {
                throw new ServiceAuthenticationException(ex.Message);
            }

            return response;
        }

        public async Task<LogoutResponse> Logout(LogoutRequest request)
        {
            LogoutResponse response = null;

            var uri = UriHelper.CombineUri(GlobalSetting.Instance.BaseGatewayEndpoint, ApiConstants.LogoutEndpoint);

            try
            {
                response = await _requestProvider.DeleteAsync<LogoutResponse>(uri, request.AuthAccessToken);

                response = new LogoutResponse()
                {
                    IsSuccessful = true,
                };
            }
            catch (System.Exception ex)
            {
                response = CreateErrorResponse<LogoutResponse>(ex);
            }

            return response;
        }
    }
}