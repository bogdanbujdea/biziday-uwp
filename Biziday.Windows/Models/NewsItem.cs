namespace Biziday.Windows.Models
{
    public class NewsItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Link { get; set; }
        public int CategoryId { get; set; }
        public int NewsType { get; set; }
        public int EditorGrade { get; set; }
        public string SourceUrl { get; set; }
        public int PublishedOn { get; set; }
        public object[] Photos { get; set; }
    }
}