using System.Threading.Tasks;
using Biziday.UWP.Validation.Reports.Web;

namespace Biziday.UWP.News
{
    public interface INewsRetriever
    {
        Task<WebDataReport<NewsInfo>> RetrieveNews();
    }
}