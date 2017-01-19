using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;
using Biziday.Core.Communication;
using Biziday.Core.Models;
using Biziday.Core.Modules.App;
using Biziday.Core.Modules.App.Analytics;
using Biziday.Core.Modules.News.Services;
using Biziday.Core.Repositories;
using NotificationsExtensions;
using NotificationsExtensions.Toasts;

namespace Biziday.NewsTask
{
    public sealed class NewsBackgroundTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            var settingsRepository = new SettingsRepository();
            var statisticsService = new StatisticsService();
            var newsRetriever = new NewsRetriever(settingsRepository, new RestClient(), statisticsService,
                new AppStateManager(settingsRepository));
            var dataReport = await newsRetriever.RetrieveNews(1);
            if (dataReport.IsSuccessful)
            {
                var previousNewsId = settingsRepository.GetData<int>(SettingsKey.LastNewsId);
                var news = dataReport.Content.Data.ToList();
                var lastNewsId = news.FirstOrDefault()?.Id;
                ShowNotification(news, "last: " + lastNewsId + ", previous: " + previousNewsId);
                if (lastNewsId != previousNewsId)
                {
                    ShowNotification(news);
                    settingsRepository.SetData(SettingsKey.LastNewsId, lastNewsId);
                }
            }
            deferral.Complete();
        }

        private void ShowNotification(List<NewsItem> news, string s = "")
        {
            var content = new ToastContent
            {
                Visual = new ToastVisual
                {
                    BindingGeneric = new ToastBindingGeneric
                    {
                        AppLogoOverride = new ToastGenericAppLogo
                        {
                            HintCrop = ToastGenericAppLogoCrop.Circle,
                            Source = "ms-appx:///Assets/NewStoreLogo.scale-100.png"
                        }
                    }
                }
            };
            if (string.IsNullOrWhiteSpace(s) == false)
                content.Visual.BindingGeneric.Children.Add(new AdaptiveText { Text = s });
            content.Visual.BindingGeneric.Children.Add(new AdaptiveText { Text = news.FirstOrDefault()?.Body });
            Show(content);
        }

        private void Show(ToastContent content)
        {
            ToastNotificationManager.CreateToastNotifier().Show(new ToastNotification(content.GetXml()));
        }
    }
}