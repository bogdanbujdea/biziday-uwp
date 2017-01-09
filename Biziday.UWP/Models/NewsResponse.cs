using System.Collections.Generic;
using Newtonsoft.Json;

namespace Biziday.UWP.Models
{
    internal class NewsResponse
    {
        [JsonProperty(PropertyName = "per_page")]
        public int PerPage { get; set; }

        [JsonProperty(PropertyName = "next_page_url")]
        public string NextPageUrl { get; set; }

        public int CurrentPage { get; set; }

        public int From { get; set; }

        public int To { get; set; }

        [JsonProperty(PropertyName = "data")]
        public ICollection<NewsItem> NewsItems { get; set; }
    }
}