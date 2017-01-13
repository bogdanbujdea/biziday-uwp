using System.Collections.Generic;
using System.Threading.Tasks;
using Biziday.UWP.Communication;
using Biziday.UWP.Modules.App;
using Biziday.UWP.Modules.News.Models;
using Biziday.UWP.Repositories;
using Biziday.UWP.Validation.Reports.Web;
using Newtonsoft.Json;

namespace Biziday.UWP.Modules.News.Services
{
    public class NewsRetriever : INewsRetriever
    {
        private readonly ISettingsRepository _settingsRepository;
        private readonly IRestClient _restClient;
        private readonly IAppStateManager _appStateManager;

        public NewsRetriever(ISettingsRepository settingsRepository, IRestClient restClient, IAppStateManager appStateManager)
        {
            _settingsRepository = settingsRepository;
            _restClient = restClient;
            _appStateManager = appStateManager;
        }

        public async Task<WebDataReport<NewsInfo>> RetrieveNews()
        {
            if (_appStateManager.FirstTimeUse())
            {
                await RegisterUser();
            }
            return await GetNews(1);
        }

        private async Task<WebDataReport<NewsInfo>> GetNews(int page)
        {
            var report = new WebDataReport<NewsInfo>();
            var pairs = CreateNewsPaginationInfo(page);

            var webReport = await _restClient.PostAsync(ApiEndpoints.GetNewsUrl, pairs);
            if (webReport.IsSuccessful)
            {
                var result = JsonConvert.DeserializeObject<NewsApiResponse>(webReport.StringResponse);
                report.Content = result.NewsInfo;
                report.IsSuccessful = true;
            }
            else
            {
                report.ErrorMessage = webReport.ErrorMessage;
            }
            return report;

        }

        private List<KeyValuePair<string, string>> CreateNewsPaginationInfo(int page)
        {
            var pairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("userId",
                    _settingsRepository.GetData<int>(SettingsKey.UserId).ToString()),
                new KeyValuePair<string, string>("last_order_date", 0.ToString()),
                new KeyValuePair<string, string>("current_page", page.ToString()),
                new KeyValuePair<string, string>("per_page", "30")
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
            }
        }       
    }
}