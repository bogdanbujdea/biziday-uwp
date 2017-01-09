using Newtonsoft.Json;

namespace Biziday.UWP.Validation.Reports.Web
{
    public class RequestStatus
    {
        [JsonProperty(PropertyName = "result")]
        public int Status { get; set; }

        public string ErrorMessage { get; set; }
    }
}