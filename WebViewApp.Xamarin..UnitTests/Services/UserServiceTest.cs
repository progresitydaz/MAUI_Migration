using System.Diagnostics;
using System.Threading.Tasks;
using StembureauApp.Xamarin.Core;
using StembureauApp.Xamarin.Core.Constants;
using StembureauApp.Xamarin.Core.Ioc;
using StembureauApp.Xamarin.Core.Services.Mocks;
using Xamarin.Forms.Mocks;
using Xunit;

namespace StembureauApp.Xamarin.UnitTests.Services
{
    public class UserServiceTest
    {
        public UserServiceTest()
        {
            GlobalSetting.Instance.BaseGatewayEndpoint = ApiConstants.BaseDevApiUrl;
            DependencyResolver.RegisterComponents(true);
            MockForms.Init();
            SettingsMockService.SetMockValues();
        }

        [Fact]
        public async void AuthenticateTest()
        {
            var testHelper = new TestHelper();

            var response = await testHelper.Login();

            Assert.True(response != null &&
                        response.IsSuccessful);

            Debug.WriteLine("AuthenticateTest called.  message is " + response.Message);
        }

        [Fact]
        public async Task VerifySmsCodeTest()
        {
            var testHelper = new TestHelper();

            var response = await testHelper.VerifySmsCode();

            Assert.True(response.IsSuccessful);

            Debug.WriteLine("VerifySmsCodeTest called");
        }
    }
}
