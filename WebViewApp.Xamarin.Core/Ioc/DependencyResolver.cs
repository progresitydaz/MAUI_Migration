using System;
using System.Globalization;
using System.Reflection;
using Autofac;
using WebViewApp.Xamarin.Core.Dependency;
using WebViewApp.Xamarin.Core.Helpers;
using WebViewApp.Xamarin.Core.Repositories;
using WebViewApp.Xamarin.Core.Serialization;
using WebViewApp.Xamarin.Core.Services;
using WebViewApp.Xamarin.Core.Services.Mocks;
using WebViewApp.Xamarin.Core.ViewModels;
using WebViewApp.Xamarin.Core.ViewModels.Base;
using Xamarin.Forms;

namespace WebViewApp.Xamarin.Core.Ioc
{
    public static class AppDependencyResolver
    {
        private static IContainer _container;

        public static void RegisterComponents(bool isMock = false)
        {
            var builder = new ContainerBuilder();

            var assembly = Assembly.GetExecutingAssembly();

            // View models
            builder.RegisterAssemblyTypes(assembly)
                  .Where(t => t.Name.EndsWith("ViewModel"));

            // Providers
            builder.RegisterType<RequestProvider>().As<IRequestProvider>();

            // Persistance
            // Persistance
            builder.RegisterType<LocalDbContextService>().As<ILocalDbContextService>()
                .WithParameter(new TypedParameter(typeof(bool), false)).SingleInstance();

            // Services
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<ApplicationDataService>().As<IApplicationDataService>();

            if (isMock)
            {
                builder.RegisterType<SettingsMockService>().As<ISettingsService>();
            }
            else
            {
                builder.RegisterType<SettingsService>().As<ISettingsService>();
                builder.RegisterType<DialogService>().As<IDialogService>();
                builder.RegisterType<NavigationService>().As<INavigationService>();
                builder.RegisterType<PhoneService>().As<IPhoneService>();
                builder.RegisterType<UIRenderService>().As<IUIRenderService>();
                builder.RegisterType<ConnectionService>().As<IConnectionService>();
                builder.RegisterType<AnalyticsService>().As<IAnalyticsService>();
                builder.RegisterType<FileSystemService>().As<IFileSystemService>();
                builder.RegisterInstance<IFileHelper>(DependencyService.Get<IFileHelper>());
                builder.RegisterInstance<IPlatformManager>(DependencyService.Get<IPlatformManager>());
            }

            _container = builder.Build();
        }

        public static object Resolve(Type resolveType)
        {
            var resolveObj = _container.Resolve(resolveType);
            return resolveObj;
        }

        public static T Resolve<T>() where T : class
        {
            var resolveObj = _container.Resolve<T>();
            return resolveObj;
        }

        public static ViewModelBase ResolveViewModel(Element view)
        {
            var viewType = view.GetType();
            var viewName = viewType.FullName.Replace(".Views.", ".ViewModels.");
            var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
            var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}Model, {1}", viewName, viewAssemblyName);

            var viewModelType = Type.GetType(viewModelName);

            if (viewModelType == null)
            {
                throw new Exception($"Cannot locate view model type for {viewModelType}");
            }

            var viewModel = (ViewModelBase)Resolve(viewModelType);

            return viewModel;
        }
    }
}
