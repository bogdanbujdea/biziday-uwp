using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Biziday.UWP.Communication;
using Biziday.UWP.Models;
using Biziday.UWP.Modules.App;
using Biziday.UWP.Modules.App.Analytics;
using Biziday.UWP.Modules.News.Models;
using Biziday.UWP.Repositories;
using Biziday.UWP.Validation.Reports.Web;
using Newtonsoft.Json;

namespace Biziday.UWP.Modules.News.Services
{
    public class NewsRetriever : IncrementalItemSourceBase<NewsItem>, INewsRetriever
    {
        private readonly ISettingsRepository _settingsRepository;
        private readonly IRestClient _restClient;
        private readonly IStatisticsService _statisticsService;
        private readonly IAppStateManager _appStateManager;
        private NewsPaginationInfo _paginationInfo;

        public NewsRetriever(ISettingsRepository settingsRepository, IRestClient restClient, IStatisticsService statisticsService,
            IAppStateManager appStateManager)
        {
            _settingsRepository = settingsRepository;
            _restClient = restClient;
            _statisticsService = statisticsService;
            _appStateManager = appStateManager;
            _paginationInfo = new NewsPaginationInfo
            {
                PerPage = 30,
                CurrentPage = 0
            };
        }

        public async Task<WebDataReport<NewsInfo>> RetrieveNews(int page)
        {
            if (_appStateManager.FirstTimeUse())
            {
                await RegisterUser();
            }
            return await GetNews(page);
        }

        private async Task<WebDataReport<NewsInfo>> GetNews(int page)
        {
            var report = new WebDataReport<NewsInfo>();

            try
            {
                _paginationInfo.CurrentPage = page;
                var pairs = CreateNewsPaginationInfo();

                var webReport = await _restClient.PostAsync(ApiEndpoints.GetNewsUrl, pairs);
                if (webReport.IsSuccessful)
                {
                    var result = JsonConvert.DeserializeObject<NewsApiResponse>(webReport.StringResponse);
                    report.Content = result.NewsInfo;
                    if (result.NewsInfo.LastOrderDate == 0 || result.NewsInfo.LastOrderDate == null)
                    {
                        RaiseHasMoreItemsChanged(false);
                        _statisticsService.RegisterEvent(EventCategory.AppEvent, "news", "finished_stream");
                    }
                    else
                        _paginationInfo.LastOrderDate = result.NewsInfo.LastOrderDate.GetValueOrDefault();
                    report.IsSuccessful = true;
                }
                else
                {
                    report.ErrorMessage = webReport.ErrorMessage;
                }
            }
            catch (Exception exception)
            {
                report.ErrorMessage = exception.Message;
            }
            return report;

        }

        private List<KeyValuePair<string, string>> CreateNewsPaginationInfo()
        {
            var pairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("userId",
                    _settingsRepository.GetData<int>(SettingsKey.UserId).ToString()),
                new KeyValuePair<string, string>("last_order_date", _paginationInfo.LastOrderDate.ToString()),
                new KeyValuePair<string, string>("current_page", _paginationInfo.CurrentPage.ToString()),
                new KeyValuePair<string, string>("per_page", _paginationInfo.PerPage.ToString())
            };
            return pairs;
        }

        private async Task RegisterUser()
        {
            var registrationData = new List<KeyValuePair<string, string>>();
            registrationData.Add(new KeyValuePair<string, string>("deviceType", "Windows tablet"));
            var webReport = await _restClient.PostAsync(ApiEndpoints.RegisterUrl, registrationData);
            if (webReport.IsSuccessful)
            {
                var registerResult = JsonConvert.DeserializeObject<RegisterResult>(webReport.StringResponse);
                _settingsRepository.SetData(SettingsKey.UserId, registerResult.UserId);
                _statisticsService.RegisterEvent(EventCategory.UserEvent, "registration", registerResult.UserId.ToString());
            }
        }

        public override async Task LoadMoreItemsAsync(ICollection<NewsItem> collection, uint suggestLoadCount)
        {
            var downloadReport = await GetNews(_paginationInfo.CurrentPage + 1);
            if (downloadReport.IsSuccessful)
            {
                foreach (var newsItem in downloadReport.Content.Data)
                {
                    if (collection.Any(c => c.Id == newsItem.Id) == false)
                        collection.Add(newsItem);
                    else
                    {
                        Debug.WriteLine("already exists: " + newsItem.Body);
                    }
                }
            }
        }

        public void Refresh()
        {
            _paginationInfo = new NewsPaginationInfo
            {
                PerPage = 30,
                CurrentPage = 0
            };
        }
    }
}