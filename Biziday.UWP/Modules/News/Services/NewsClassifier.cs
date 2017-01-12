using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biziday.UWP.Communication;
using Biziday.UWP.Modules.News.Models;
using Biziday.UWP.Validation.Reports.Web;
using Newtonsoft.Json;

namespace Biziday.UWP.Modules.News.Services
{
    public class NewsClassifier : INewsClassifier
    {
        private readonly IRestClient _restClient;
        public static int AreaExternEurope = 1001;
        public static int AreaMoldova = 1000;
        public static int AreaOtherContinent = 1002;

        public List<int> SpecialAreas => new List<int> { AreaExternEurope, AreaMoldova, AreaOtherContinent };

        public NewsClassifier(IRestClient restClient)
        {
            _restClient = restClient;
        }

        public async Task<WebDataReport<List<Area>>> RetrieveAreas()
        {
            var report = new WebDataReport<List<Area>>();
            var areasWebReport = await _restClient.PostAsync(ApiEndpoints.GetAreasUrl, new List<KeyValuePair<string, string>>());
            if (areasWebReport.IsSuccessful)
            {
                var areasResponse = JsonConvert.DeserializeObject<AreasResponse>(areasWebReport.StringResponse);
                report.Content = areasResponse.Areas.Where(a => SpecialAreas.Any(areaId => a.Id == areaId) == false).ToList();
            }
            else
            {
                report.ErrorMessage = areasWebReport.ErrorMessage;
            }
            return report;
        }
    }
}