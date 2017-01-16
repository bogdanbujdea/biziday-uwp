using System.Collections.Generic;
using Biziday.Core.Models;
using Newtonsoft.Json;

namespace Biziday.Core.Modules.News.Models
{
    public class RegisterResult
    {
        public int Result { get; set; }
        public int UserId { get; set; }
    }

    public class NewsApiResponse
    {
        public int Result { get; set; }
        [JsonProperty(PropertyName = "News")]
        public NewsInfo NewsInfo { get; set; }
    }

    public class NewsInfo
    {
        public int PerPage { get; set; }
        public int CurrentPage { get; set; }
        public string NextPageUrl { get; set; }
        public string PrevPageUrl { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public IEnumerable<NewsItem> Data { get; set; }

        [JsonProperty(PropertyName = "last_order_date")]
        public int? LastOrderDate { get; set; }
    }
}