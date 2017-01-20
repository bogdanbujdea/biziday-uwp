using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Windows.ApplicationModel.Activation;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Biziday.Core.Communication;
using Biziday.Core.Modules.App;
using Biziday.Core.Modules.App.Analytics;
using Biziday.Core.Modules.App.Dialogs;
using Biziday.Core.Modules.App.Navigation;
using Biziday.Core.Modules.News.Services;
using Biziday.Core.Repositories;
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

        private async void AppUnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            await new MessageDialog("A aparut o eroare, va rugam reincercati " + e.Exception.Message).ShowAsync();
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
            _container.RegisterPerRequest(typeof(IUserNotificationService), "IUserNotificationService", typeof(UserNotificationService));
            _container.RegisterPerRequest(typeof(IStatisticsService), "IStatisticsService", typeof(StatisticsService));
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

        protected override void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);
            if (args.Kind == ActivationKind.ToastNotification && args is ToastNotificationActivatedEventArgs)
            {
                var data = args as ToastNotificationActivatedEventArgs;
                if (args.PreviousExecutionState != ApplicationExecutionState.Running)
                {
                    DisplayRootView<HomeView>(data.Argument);
                    return;
                }
                var settingsRepository = IoC.Get<ISettingsRepository>();
                settingsRepository.SetLocalData(SettingsKey.ToastNewsId, data.Argument);
                var statisticsService = IoC.Get<IStatisticsService>();
                statisticsService.RegisterEvent(EventCategory.AppEvent, "toast_activation", data.Argument);
                DisplayRootView<HomeView>();
            }
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