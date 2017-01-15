using System.Collections.Generic;
using System.Threading.Tasks;
using Biziday.Core.Models;
using Biziday.Core.Modules.News.Models;
using Biziday.Core.Validation.Reports.Web;

namespace Biziday.Core.Modules.News.Services
{
    public interface INewsRetriever
    {
        Task<WebDataReport<NewsInfo>> RetrieveNews(int page);

        Task LoadMoreItemsAsync(ICollection<NewsItem> collection, uint suggestLoadCount);
        void Refresh();
    }
}