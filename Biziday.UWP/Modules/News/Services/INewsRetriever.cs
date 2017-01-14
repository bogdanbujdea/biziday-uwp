using System.Collections.Generic;
using System.Threading.Tasks;
using Biziday.UWP.Models;
using Biziday.UWP.Modules.News.Models;
using Biziday.UWP.Validation.Reports.Web;

namespace Biziday.UWP.Modules.News.Services
{
    public interface INewsRetriever
    {
        Task<WebDataReport<NewsInfo>> RetrieveNews(int page);

        Task LoadMoreItemsAsync(ICollection<NewsItem> collection, uint suggestLoadCount);
        void Refresh();
    }
}