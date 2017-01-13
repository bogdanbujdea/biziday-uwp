using System.Collections.Generic;
using System.Threading.Tasks;
using Biziday.UWP.Validation.Reports.Web;

namespace Biziday.UWP.Communication
{
    public interface IRestClient
    {
        Task<BasicWebReport> GetAsync(string url, bool useAuthenticatation = false);

        Task<BasicWebReport> PostAsync(string url, List<KeyValuePair<string, string>> formData);
        Task<BasicWebReport> PostJsonAsync(object content, string url);
    }
}