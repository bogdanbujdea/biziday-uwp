using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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
        private ObservableCollection<NewsItem> _newsItems;
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
                await Task.Delay(14000);
                LocationIsSelected = _appStateManager.LocationIsSelected();
                var report = await _newsRetriever.RetrieveNews();
                if (report.IsSuccessful)
                {
                    NewsItems = new ObservableCollection<NewsItem>(report.Content.Data);
                }
                else await _userNotificationService.ShowErrorMessageDialogAsync(report.ErrorMessage);
            }
            catch (Exception)
            {

            }
            finally
            {
                EndWebRequest();
            }
        }

        public ObservableCollection<NewsItem> NewsItems
        {
            get { return _newsItems; }
            set
            {
                if (Equals(value, _newsItems)) return;
                _newsItems = value;
                NotifyOfPropertyChange(() => NewsItems);
            }
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