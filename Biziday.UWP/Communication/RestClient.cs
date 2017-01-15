using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Web;
using Windows.Web.Http;
using Biziday.UWP.Validation.Reports.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using HttpClient = Windows.Web.Http.HttpClient;
using HttpResponseMessage = Windows.Web.Http.HttpResponseMessage;

namespace Biziday.UWP.Communication
{
    public class RestClient : IRestClient
    {
        private readonly HttpClient _httpClient;
        private JsonSerializerSettings _jsonSerializerSettings;

        public RestClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<BasicWebReport> GetAsync(string url, bool useAuthenticatation = false)
        {
            if (useAuthenticatation)
            {
            }
            var responseMessage = await _httpClient.GetAsync(new Uri(url));
            var httpReport = await CreateHttpReport(responseMessage);

            return httpReport;
        }

        public async Task<BasicWebReport> PostAsync(string url, List<KeyValuePair<string, string>> formData)
        {
            BasicWebReport httpReport = new BasicWebReport();
            try
            {
                var content = new HttpFormUrlEncodedContent(formData);

                var responseMessage = await _httpClient.PostAsync(new Uri(url), content);
                httpReport = await CreateHttpReport(responseMessage);
            }
            catch (Exception exception)
            {
                httpReport.ErrorMessage = GetErrorMessageFromWebException(exception);
            }
            return httpReport;
        }

        private string GetErrorMessageFromWebException(Exception exception)
        {
            var webErrorStatus = WebError.GetStatus(exception.HResult);
            if (webErrorStatus == WebErrorStatus.CannotConnect)
                return "Va rugam verificati conexiunea la internet.";
            return exception.Message;
        }

        public async Task<BasicWebReport> PostJsonAsync(object content, string url)
        {
            BasicWebReport httpReport = new BasicWebReport();
            try
            {
                var json = JsonConvert.SerializeObject(content, Formatting.None, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                var httpStringContent = new HttpStringContent(json, UnicodeEncoding.Utf8, "application/json");
                var responseMessage = await _httpClient.PostAsync(new Uri(url), httpStringContent);

                httpReport = await CreateHttpReport(responseMessage);
                return httpReport;
            }
            catch (Exception exception)
            {
                httpReport.ErrorMessage = GetErrorMessageFromWebException(exception);
            }
            return httpReport;
        }

        private static async Task<BasicWebReport> CreateHttpReport(HttpResponseMessage responseMessage)
        {
            var httpReport = new BasicWebReport
            {
                HttpCode = responseMessage.StatusCode,
                StringResponse = await responseMessage.Content.ReadAsStringAsync()
            };
            try
            {
                httpReport.RequestStatus = JsonConvert.DeserializeObject<RequestStatus>(httpReport.StringResponse);
                if (httpReport.RequestStatus != null)
                {
                    httpReport.IsSuccessful = httpReport.RequestStatus.Status == 0;
                    if (httpReport.IsSuccessful == false)
                        httpReport.ErrorMessage = httpReport.RequestStatus.ErrorMessage;
                }
            }
            catch (Exception exception)
            {
            }

            return httpReport;
        }
    }
}