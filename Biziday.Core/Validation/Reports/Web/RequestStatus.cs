using Newtonsoft.Json;

namespace Biziday.Core.Validation.Reports.Web
{
    public class RequestStatus
    {
        [JsonProperty(PropertyName = "result")]
        public int Status { get; set; }

        public string ErrorMessage { get; set; }
    }
}