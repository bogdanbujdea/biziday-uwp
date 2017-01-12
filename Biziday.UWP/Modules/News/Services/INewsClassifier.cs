using System.Collections.Generic;
using System.Threading.Tasks;
using Biziday.UWP.Modules.News.Models;
using Biziday.UWP.Validation.Reports.Web;

namespace Biziday.UWP.Modules.News.Services
{
    public interface INewsClassifier
    {
        List<int> SpecialAreas { get; }

        Task<WebDataReport<List<Area>>> RetrieveAreas();
    }
}