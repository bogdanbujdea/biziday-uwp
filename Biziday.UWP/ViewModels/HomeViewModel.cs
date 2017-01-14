using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.ApplicationModel.Email;
using Windows.System;
using Biziday.UWP.Models;
using Biziday.UWP.Modules.App;
using Biziday.UWP.Modules.App.Dialogs;
using Biziday.UWP.Modules.App.Navigation;
using Biziday.UWP.Modules.News.Services;

namespace Biziday.UWP.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly INewsRetriever _newsRetriever;
        private readonly IAppStateManager _appStateManager;
        private readonly IPageNavigationService _pageNavigationService;
        private readonly IUserNotificationService _userNotificationService;
        private IncrementalLoadingCollection<NewsItem, NewsRetriever> _newsItems;
        private bool _pageIsLoading;
        private bool _locationIsSelected;

        public HomeViewModel(INewsRetriever newsRetriever, IAppStateManager appStateManager,
            IPageNavigationService pageNavigationService, IUserNotificationService userNotificationService)
        {
            _newsRetriever = newsRetriever;
            _appStateManager = appStateManager;
            _pageNavigationService = pageNavigationService;
            _userNotificationService = userNotificationService;
        }

        protected override async void OnActivate()
        {
            base.OnActivate();
            try
            {
                StartWebRequest();
                if (_appStateManager.FirstTimeUse())
                {
                    LocationIsSelected = false;
                    await _userNotificationService.ShowMessageDialogAsync("Disclaimer: Aceasta nu este aplicatia oficiala Biziday pentru platforma Windows.");
                    await _newsRetriever.RetrieveNews(1);
                }
                else
                    LocationIsSelected = _appStateManager.LocationIsSelected();
                NewsItems = new IncrementalLoadingCollection<NewsItem, NewsRetriever>(_newsRetriever as NewsRetriever);
            }
            catch (Exception)
            {

            }
            finally
            {
                EndWebRequest();
            }
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
        }

        public async Task SendFeedback()
        {
            var emailMessage = new EmailMessage {Subject = "Feedback Biziday"};
            emailMessage.To.Add(new EmailRecipient("bogdan@thewindev.net"));
            await EmailManager.ShowComposeNewEmailAsync(emailMessage);
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
        }

        public void PageLoaded()
        {
            PageIsLoading = false;
        }
    }
}