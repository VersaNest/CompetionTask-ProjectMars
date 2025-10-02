namespace Selenium_ProjectMars.Models
{
    public class AppConfigModel
    {
        public string Url { get; set; }
        public ReportSettings Report { get; set; }
    }

    public class ReportSettings
    {
        public string Path { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
    }
}