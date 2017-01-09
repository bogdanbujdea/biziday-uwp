using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Storage.Streams;
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
/*
        private void AddAuthentication()
        {
            _httpClient.DefaultRequestHeaders.Remove("Authentication");
            var token = _appSettings.Get<string>(StorageKey.LoginToken);
            _httpClient.DefaultRequestHeaders.Add("Authentication", token);
        }*/

        public async Task<BasicWebReport> RegisterAsync(object model, string url, bool useAuthenticatation = false, bool serialize = true)
        {
            var pairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("deviceType", "Android tablet")
            };

            var content = new HttpFormUrlEncodedContent(pairs);
            
            var responseMessage = await _httpClient.PostAsync(new Uri(url), content);

            var httpReport = await CreateHttpReport(responseMessage);
            return httpReport;
        }

        public async Task<BasicWebReport> PostAsync(string url, List<KeyValuePair<string, string>> formData)
        {
            var content = new HttpFormUrlEncodedContent(formData);

            var responseMessage = await _httpClient.PostAsync(new Uri(url), content);

            var httpReport = await CreateHttpReport(responseMessage);
            return httpReport;
        }

        private static async Task<BasicWebReport> CreateHttpReport(HttpResponseMessage responseMessage)
        {
            var httpReport = new BasicWebReport
            {
                HttpCode = responseMessage.StatusCode,
                StringResponse = await responseMessage.Content.ReadAsStringAsync(),
                IsSuccessful = responseMessage.IsSuccessStatusCode
            };
            try
            {
                httpReport.RequestStatus = JsonConvert.DeserializeObject<RequestStatus>(httpReport.StringResponse);
            }
            catch (Exception exception)
            {
            }

            return httpReport;
        }
    }
}