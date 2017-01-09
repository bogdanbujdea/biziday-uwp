using System.Collections.Generic;
using System.Threading.Tasks;
using Biziday.UWP.Communication;
using Biziday.UWP.Repositories;
using Biziday.UWP.Validation.Reports.Operation;
using Biziday.UWP.Validation.Reports.Web;
using Newtonsoft.Json;

namespace Biziday.UWP.News
{
    public class NewsRetriever : INewsRetriever
    {
        private readonly ISettingsRepository _settingsRepository;
        private readonly IRestClient _restClient;

        public NewsRetriever(ISettingsRepository settingsRepository, IRestClient restClient)
        {
            _settingsRepository = settingsRepository;
            _restClient = restClient;
        }

        public async Task<WebDataReport<NewsInfo>> RetrieveNews()
        {
            if (FirstTimeUse())
            {
                await RegisterUser();
            }
            return await GetNews(1);
        }

        private async Task<WebDataReport<NewsInfo>> GetNews(int page)
        {
            var report = new WebDataReport<NewsInfo>();
            var pairs = new List<KeyValuePair<string, string>>();
            pairs.Add(new KeyValuePair<string, string>("userId", _settingsRepository.GetData<int>(SettingsKey.UserId).ToString()));
            pairs.Add(new KeyValuePair<string, string>("last_order_date", 0.ToString()));
            pairs.Add(new KeyValuePair<string, string>("current_page", page.ToString()));
            pairs.Add(new KeyValuePair<string, string>("per_page", "30"));

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

        private async Task RegisterUser()
        {
            var webReport = await _restClient.RegisterAsync("deviceType=Windows%20tablet", ApiEndpoints.RegisterUrl, serialize: false);
            if (webReport.IsSuccessful)
            {
                var registerResult = JsonConvert.DeserializeObject<RegisterResult>(webReport.StringResponse);
                _settingsRepository.SetData(SettingsKey.UserId, registerResult.UserId);
                //var areas = await _restClient.PostAsync(ApiEndpoints.GetAreasUrl);
            }
        }

        private bool FirstTimeUse()
        {
            var userId = _settingsRepository.GetData<int>(SettingsKey.UserId);
            return userId == 0;
        }
    }

    public class NewsReport
    {
    }
}