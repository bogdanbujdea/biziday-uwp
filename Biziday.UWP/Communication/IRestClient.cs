using System.Collections.Generic;
using System.Threading.Tasks;
using Biziday.UWP.Validation.Reports.Web;

namespace Biziday.UWP.Communication
{
    public interface IRestClient
    {
        Task<BasicWebReport> GetAsync(string url, bool useAuthenticatation = false);

        Task<BasicWebReport> RegisterAsync(object model, string url, bool useAuthenticatation = false, bool serialize = true);
        Task<BasicWebReport> PostAsync(string url, List<KeyValuePair<string, string>> formData);
    }
}