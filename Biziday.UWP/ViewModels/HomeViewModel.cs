using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Email;
using Windows.System;
using Biziday.Core.Models;
using Biziday.Core.Modules.App;
using Biziday.Core.Modules.App.Analytics;
using Biziday.Core.Modules.App.Dialogs;
using Biziday.Core.Modules.App.Navigation;
using Biziday.Core.Modules.News.Services;
using Caliburn.Micro;

namespace Biziday.UWP.ViewModels
{
    public class HomeViewModel : ViewModelBase, IHandle<LocationChangedEvent>
    {
        private readonly INewsRetriever _newsRetriever;
        private readonly IAppStateManager _appStateManager;
        private readonly IPageNavigationService _pageNavigationService;
        private readonly IUserNotificationService _userNotificationService;
        private readonly IStatisticsService _statisticsService;
        private IncrementalLoadingCollection<NewsItem, NewsRetriever> _newsItems;
        private bool _pageIsLoading;
        private bool _locationIsSelected;

        public HomeViewModel(INewsRetriever newsRetriever, IAppStateManager appStateManager,
            IPageNavigationService pageNavigationService, IUserNotificationService userNotificationService, IStatisticsService statisticsService, IEventAggregator eventAggregator)
        {
            _newsRetriever = newsRetriever;
            _appStateManager = appStateManager;
            _pageNavigationService = pageNavigationService;
            _userNotificationService = userNotificationService;
            _statisticsService = statisticsService;
            eventAggregator.Subscribe(this);
        }

        protected override async void OnInitialize()
        {
            base.OnInitialize();
            try
            {
                StartWebRequest();
                if (_appStateManager.FirstTimeUse())
                {
                    LocationIsSelected = false;
                    await _userNotificationService.ShowMessageDialogAsync("Disclaimer: Aceasta nu este aplicația oficială Biziday pentru platforma Windows.");
                    await _newsRetriever.RetrieveNews(1);
                }
                else
                    LocationIsSelected = _appStateManager.LocationIsSelected();
                NewsItems = new IncrementalLoadingCollection<NewsItem, NewsRetriever>(_newsRetriever as NewsRetriever);
            }
            finally
            {
                _statisticsService.RegisterPage("news");
                EndWebRequest();
            }
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            LocationIsSelected = _appStateManager.LocationIsSelected();
        }

        public IncrementalLoadingCollection<NewsItem, NewsRetriever> NewsItems
        {
            get { return _newsItems; }
            set
            {
                if (Equals(value, _newsItems)) return;
                _newsItems = value;
                NotifyOfPropertyChange(() => NewsItems);
            }
        }

        public async Task SetRating()
        {
            await Launcher.LaunchUriAsync(new Uri(string.Format("ms-windows-store:REVIEW?PFN={0}", Windows.ApplicationModel.Package.Current.Id.FamilyName)));
            _statisticsService.RegisterEvent(EventCategory.AppEvent, "rating", DateTime.Now.ToString("F"));
        }

        public async Task SendFeedback()
        {
            var emailMessage = new EmailMessage { Subject = "Feedback Biziday" };
            emailMessage.To.Add(new EmailRecipient("bogdan@thewindev.net"));
            await EmailManager.ShowComposeNewEmailAsync(emailMessage);
            _statisticsService.RegisterEvent(EventCategory.AppEvent, "feedback", DateTime.Now.ToString("F"));
        }

        public void RefreshNews()
        {
            _newsRetriever.Refresh();
            NewsItems = new IncrementalLoadingCollection<NewsItem, NewsRetriever>(_newsRetriever as NewsRetriever);
        }

        public bool PageIsLoading
        {
            get { return _pageIsLoading; }
            set
            {
                if (value == _pageIsLoading) return;
                _pageIsLoading = value;
                NotifyOfPropertyChange(() => PageIsLoading);
            }
        }

        public bool LocationIsSelected
        {
            get { return _locationIsSelected; }
            set
            {
                if (value == _locationIsSelected) return;
                _locationIsSelected = value;
                NotifyOfPropertyChange(() => LocationIsSelected);
            }
        }

        public void SelectLocation()
        {
            _pageNavigationService.NavigateTo<LocationViewModel>();
        }

        public void LoadingPage()
        {
            PageIsLoading = true;
            _statisticsService.RegisterEvent(EventCategory.AppEvent, "news", "page_loading");
        }

        public void PageLoaded()
        {
            PageIsLoading = false;
            _statisticsService.RegisterEvent(EventCategory.AppEvent, "news", "page_loaded");
        }

        public void Handle(LocationChangedEvent message)
        {
            RefreshNews();
        }
    }
}