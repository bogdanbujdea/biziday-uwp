using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biziday.UWP.Communication;
using Biziday.UWP.Modules.News.Models;
using Biziday.UWP.Repositories;
using Biziday.UWP.Validation.Reports.Web;
using Newtonsoft.Json;

namespace Biziday.UWP.Modules.News.Services
{
    public class NewsClassifier : INewsClassifier
    {
        private readonly IRestClient _restClient;
        private readonly ISettingsRepository _settingsRepository;
        public static int AreaExternEurope = 1001;
        public static int AreaMoldova = 1000;
        public static int AreaOtherContinent = 1002;

        public List<int> SpecialAreas => new List<int> { AreaExternEurope, AreaMoldova, AreaOtherContinent };

        public NewsClassifier(IRestClient restClient, ISettingsRepository settingsRepository)
        {
            _restClient = restClient;
            _settingsRepository = settingsRepository;
        }

        public async Task<WebDataReport<List<Area>>> RetrieveAreas()
        {
            var report = new WebDataReport<List<Area>>();
            var areasWebReport =
                await _restClient.PostAsync(ApiEndpoints.GetAreasUrl, new List<KeyValuePair<string, string>>());
            if (areasWebReport.IsSuccessful)
            {
                var areasResponse = JsonConvert.DeserializeObject<AreasResponse>(areasWebReport.StringResponse);
                report.Content =
                    areasResponse.Areas.Where(a => SpecialAreas.Any(areaId => a.Id == areaId) == false).ToList();
            }
            else
            {
                report.ErrorMessage = areasWebReport.ErrorMessage;
            }
            return report;
        }

        public async Task<BasicWebReport> SelectArea(int areaId)
        {
            var userId = _settingsRepository.GetData<int>(SettingsKey.UserId);
            var content = new AreaConfiguration { Areas = new List<int>() { areaId }, UserId = userId };
            return await _restClient.PostJsonAsync(content, ApiEndpoints.SelectAreaURL);
        }
    }
}