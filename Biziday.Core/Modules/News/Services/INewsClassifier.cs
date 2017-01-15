using System.Collections.Generic;
using System.Threading.Tasks;
using Biziday.Core.Modules.News.Models;
using Biziday.Core.Validation.Reports.Web;

namespace Biziday.Core.Modules.News.Services
{
    public interface INewsClassifier
    {
        List<int> SpecialAreas { get; }

        Task<WebDataReport<List<Area>>> RetrieveAreas();

        Task<BasicWebReport> SelectArea(int areaId);
    }
}