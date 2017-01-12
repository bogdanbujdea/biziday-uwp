using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;
using Biziday.UWP.Communication;
using Biziday.UWP.Modules.App;
using Biziday.UWP.Modules.App.Navigation;
using Biziday.UWP.Modules.News.Services;
using Biziday.UWP.Repositories;
using Biziday.UWP.ViewModels;
using Biziday.UWP.Views;
using Caliburn.Micro;

namespace Biziday.UWP
{
    public sealed partial class App
    {
        private WinRTContainer _container;

        public App()
        {
            InitializeComponent();
            UnhandledException += AppUnhandledException;
        }

        private void AppUnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            e.Handled = true;
        }

        protected override void Configure()
        {
            _container = new WinRTContainer();

            _container.RegisterWinRTServices();
            _container.RegisterPerRequest(typeof(ISettingsRepository), "ISettingsRepository", typeof(SettingsRepository));
            _container.RegisterPerRequest(typeof(IRestClient), "IRestClient", typeof(RestClient));
            _container.RegisterPerRequest(typeof(INewsRetriever), "INewsRetriever", typeof(NewsRetriever));
            _container.RegisterPerRequest(typeof(IAppStateManager), "IAppStateManager", typeof(AppStateManager));
            _container.RegisterPerRequest(typeof(INewsClassifier), "INewsClassifier", typeof(NewsClassifier));
            _container.RegisterPerRequest(typeof(IPageNavigationService), "IPageNavigationService", typeof(PageNavigationService));
            _container.PerRequest<HomeViewModel>();
            _container.PerRequest<LocationViewModel>();
        }

        protected override void PrepareViewFirst(Frame rootFrame)
        {
            _container.RegisterNavigationService(rootFrame);
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            DisplayRootView<HomeView>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }
}