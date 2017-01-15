namespace Biziday.Core.Validation.Reports.Web
{
    public class WebDataReport<T> : BasicWebReport
    {
        private T _content;

        public T Content
        {
            get { return _content; }
            set
            {
                IsSuccessful = true;
                _content = value;
            }
        }
    }
}