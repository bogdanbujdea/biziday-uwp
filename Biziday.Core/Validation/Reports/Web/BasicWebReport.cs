using Windows.Web.Http;
using Biziday.Core.Validation.Reports.Operation;

namespace Biziday.Core.Validation.Reports.Web
{
    public class BasicWebReport: BasicReport
    {
        private RequestStatus _requestStatus;

        public HttpStatusCode HttpCode { get; set; }

        public string StringResponse { get; set; }

        public RequestStatus RequestStatus
        {
            get { return _requestStatus; }
            set
            {
                _requestStatus = value;
                if (_requestStatus.Status == 0)
                    IsSuccessful = true;
                else ErrorMessage = _requestStatus.ErrorMessage;
            }
        }
    }
}