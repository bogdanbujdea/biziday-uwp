using System.Collections.Generic;
using Biziday.UWP.Models;
using Newtonsoft.Json;

namespace Biziday.UWP.News
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
        public int LastOrderDate { get; set; }
    }
}