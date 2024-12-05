using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StembureauApp.Xamarin.Core;
using StembureauApp.Xamarin.Core.Constants;
using StembureauApp.Xamarin.Core.Ioc;
using StembureauApp.Xamarin.Core.Models;
using StembureauApp.Xamarin.Core.Services;
using StembureauApp.Xamarin.Core.Services.Mocks;
using Xamarin.Forms.Mocks;
using Xunit;

namespace StembureauApp.Xamarin.UnitTests.Services
{
    public class ApplicationDataServiceTest
    {
        public ApplicationDataServiceTest()
        {
            GlobalSetting.Instance.BaseGatewayEndpoint = ApiConstants.BaseDevApiUrl;
            DependencyResolver.RegisterComponents(true);
            MockForms.Init();
            SettingsMockService.SetMockValues();
        }

        [Fact]
        public async void GetAppConfigurationTest()
        {
            var settingsService = DependencyResolver.Resolve<ISettingsService>();
            string tokenString = await GetToken();
            SettingsMockService.SetToken(tokenString);

            var userService = DependencyResolver.Resolve<IUserService>();
            var applicationDataService = DependencyResolver.Resolve<IApplicationDataService>();

            var appConfig = await applicationDataService.GetAppConfiguration();

            Assert.True(appConfig != null &&
                        appConfig.IsSuccessful);

        }

        private async Task<string> GetToken()
        {
            var testHelper = new TestHelper();

            return await testHelper.GetToken();
        }

        [Fact]
        public async void GetScreenTest()
        {
            var settingsService = DependencyResolver.Resolve<ISettingsService>();
            string tokenString = await GetToken();
            SettingsMockService.SetToken(tokenString);

            var userService = DependencyResolver.Resolve<IUserService>();
            var applicationDataService = DependencyResolver.Resolve<IApplicationDataService>();

            var appConfig = await applicationDataService.GetAppConfiguration();

            var getScreenRequest = new GetScreenRequest
            {
                PollingStationId = appConfig.AppConfig.CurrentPollingStation.Id
            };

            var screen = await applicationDataService.GetScreen(getScreenRequest);

            Assert.True(screen != null &&
                        screen.IsSuccessful);

        }

        [Fact]
        public async void SaveScreenTest()
        {
            var settingsService = DependencyResolver.Resolve<ISettingsService>();
            string tokenString = await GetToken();
            SettingsMockService.SetToken(tokenString);

            var userService = DependencyResolver.Resolve<IUserService>();
            var applicationDataService = DependencyResolver.Resolve<IApplicationDataService>();

            var appConfig = await applicationDataService.GetAppConfiguration();

            var getScreenRequest = new GetScreenRequest
            {
                PollingStationId = appConfig.AppConfig.CurrentPollingStation.Id
            };

            var screen = await applicationDataService.GetScreen(getScreenRequest);

            foreach (var item in screen.Screen.Items)
            {
                switch (item.ItemType)
                {
                    case UIControlTypes.CHECKBOX_INPUT:
                        item.Value = item.IsSelected ? "true" : "false";
                        break;
                    case UIControlTypes.TEXT_INPUT:
                        item.Value = "true";
                        break;
                    case UIControlTypes.SUBMIT_BUTTON:
                    case UIControlTypes.NEXT_BUTTON:
                    case UIControlTypes.PREVIOUS_BUTTON:
                    case UIControlTypes.LOGOUT_BUTTON:
                        item.Value = "true";
                        break;
                    case UIControlTypes.TEXT_LABEL:
                        item.Value = "true";
                        break;
                    case UIControlTypes.IMAGE:
                        item.Value = "true";
                        break;
                    case UIControlTypes.FORM_TEXT_INPUT:
                        break;
                    case UIControlTypes.HIGHLIGHTED_LARGE_LABEL:
                        break;
                    case UIControlTypes.HIGHLIGHTED_SMALL_LABEL:
                        break;
                    case UIControlTypes.NAME_VALUE:
                        break;
                    case UIControlTypes.DATE_INPUT:
                        break;
                    case UIControlTypes.RADIO_INPUT:
                        break;
                    case UIControlTypes.NONE:
                        break;
                }
            }

            var saveScreenRequest = new SaveScreenRequest
            {
                PollingStationId = getScreenRequest.PollingStationId,
                Screen = screen.Screen,
            };


            var result = await applicationDataService.SaveScreen(saveScreenRequest);

            Assert.True(result != null &&
                        result.IsSuccessful);

        }

        [Fact]
        public async void CheckChat()
        {
            string tokenString = await GetToken();
            SettingsMockService.SetToken(tokenString);

            CheckChatRequest request = new CheckChatRequest()
            {

            };

            var applicationDataService = DependencyResolver.Resolve<IApplicationDataService>();

            var response = await applicationDataService.CheckChat(request);

            Assert.True(response != null &&
                        response.IsSuccessful);
        }

        [Fact]
        public async void GetNewChat()
        {
            string tokenString = await GetToken();
            SettingsMockService.SetToken(tokenString);

            GetNewChatRequest request = new GetNewChatRequest()
            {

            };

            var applicationDataService = DependencyResolver.Resolve<IApplicationDataService>();

            var response = await applicationDataService.GetNewChat(request);

            Assert.True(response != null &&
                        response.IsSuccessful &&
                        response.Messages.Any());
        }

        [Fact]
        public async void GetAllChat()
        {
            string tokenString = await GetToken();
            SettingsMockService.SetToken(tokenString);

            GetAllChatRequest request = new GetAllChatRequest()
            {

            };

            var applicationDataService = DependencyResolver.Resolve<IApplicationDataService>();

            var response = await applicationDataService.GetAllChat(request);

            Assert.True(response != null &&
                        response.IsSuccessful &&
                        response.Messages.Any());
        }

        [Fact]
        public async void SendChat()
        {
            string tokenString = await GetToken();
            SettingsMockService.SetToken(tokenString);

            SendChatRequest request = new SendChatRequest()
            {
                Message = new TextMessage() { Text = "Hi NL" },
                Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        Name = "upload.jpg",
                        MimeType = "image/jpeg",
                        Content = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8/5/hPwAIAgL/4d1j8wAAAABJRU5ErkJggg=="
                    }
                }
            };

            var applicationDataService = DependencyResolver.Resolve<IApplicationDataService>();

            var response = await applicationDataService.SendChat(request);

            Assert.True(response != null &&
                        response.IsSuccessful);

        }

        [Fact]
        public async void UpdateDeviceToken()
        {
            string tokenString = await GetToken();
            SettingsMockService.SetToken(tokenString);

            UpdateDeviceTokenRequest request = new UpdateDeviceTokenRequest()
            {
                DevicePlatform = "android",
                DeviceToken = "dhnZ6KjKf-A:APA91bHLX3CF0Y8LkPbV3w-6jyDT5t3buuGbiS9ve5gXo1JH0Ca4MGWvgCNHja0zgBGr-32X1bWfRgQL10YlGgih_oziviHtfiN9nTk3_Md-ud7DVnMdls-2vypEI8BDd79vOBH5165L"
            };

            var applicationDataService = DependencyResolver.Resolve<IApplicationDataService>();

            var response = await applicationDataService.UpdateDeviceToken(request);

            Assert.True(response != null &&
                        response.IsSuccessful && response.IsSaved);
        }
    }
}
