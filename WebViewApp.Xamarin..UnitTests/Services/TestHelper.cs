using System;
using System.Threading.Tasks;
using StembureauApp.Xamarin.Core.Ioc;
using StembureauApp.Xamarin.Core.Models;
using StembureauApp.Xamarin.Core.Services;
using StembureauApp.Xamarin.Core.Tests;

namespace StembureauApp.Xamarin.UnitTests.Services
{
    public class TestHelper
    {
        public async Task<AuthenticationResponse> Login()
        {
            var settingsService = DependencyResolver.Resolve<ISettingsService>();
            var userService = DependencyResolver.Resolve<IUserService>();

            var authRequest = new AuthenticationRequest()
            {
                Password = TestData.Password,
                UserName = TestData.UserName,
                AppVersion = TestData.AppVersion,
                DevicePlatform = TestData.DevicePlatform,
                DeviceToken = TestData.DeviceToken,
            };

            var response = await userService.Authenticate(authRequest);

            return response;
        }

        public async Task<SmsVerificationResponse> VerifySmsCode()
        {
            SmsVerificationResponse response = null;

            var settingsService = DependencyResolver.Resolve<ISettingsService>();
            var userService = DependencyResolver.Resolve<IUserService>();

            var loginResponse = await Login();

            if (loginResponse.IsSuccessful)
            {
                var request = new SmsVerificationRequest()
                {
                    Password = TestData.Password,
                    UserName = TestData.UserName,
                    Token1 = TestData.Token1,
                    Token2 = TestData.Token2,
                    DevicePlatform = TestData.DevicePlatform,
                    DeviceToken = TestData.DeviceToken,
                };

                response = await userService.VerifySmsToken(request);
            }

            return response;
        }

        public async Task<string> GetToken()
        {
            string token = string.Empty;

            var smsVerifyResponse = await VerifySmsCode();

            if (smsVerifyResponse.IsSuccessful)
            {
                token = smsVerifyResponse.RefreshToken;
            }

            return token;
        }
    }
}
