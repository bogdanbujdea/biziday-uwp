using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;
using Biziday.UWP.Communication;
using Biziday.UWP.News;
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
        }

        protected override void Configure()
        {
            _container = new WinRTContainer();

            _container.RegisterWinRTServices();
            _container.RegisterPerRequest(typeof(ISettingsRepository), "ISettingsRepository", typeof(SettingsRepository));
            _container.RegisterPerRequest(typeof(IRestClient), "IRestClient", typeof(RestClient));
            _container.RegisterPerRequest(typeof(INewsRetriever), "INewsRetriever", typeof(NewsRetriever));
            _container.PerRequest<HomeViewModel>();
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