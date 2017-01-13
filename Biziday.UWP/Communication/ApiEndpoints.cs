namespace Biziday.UWP.Communication
{
    public static class ApiEndpoints
    {
        public const string WebBaseUrl = "http://37.221.162.217/api";

        public const string RegisterUrl = WebBaseUrl + "/registerUser";

        public const string GetAreasUrl = WebBaseUrl + "/getAllAreas";

        public const string GetNewsUrl = WebBaseUrl + "/getNews";

        public const string SelectAreaURL = WebBaseUrl + "/configureAreas";
    }
}