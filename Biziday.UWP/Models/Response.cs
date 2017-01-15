using Newtonsoft.Json;

namespace Biziday.UWP.Models
{
    internal class Response
    {
        public int Result { get; set; }

        [JsonProperty(PropertyName = "news")]
        public NewsResponse NewsResponse { get; set; }
    }
}