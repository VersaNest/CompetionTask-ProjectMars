using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using Selenium_ProjectMars.Models;


namespace SeleniumProjectMars.Utilities
{
    public static class ExtentReportManager
    {
        private static ExtentReports extentReport;
        private static ExtentTest extentTest;
        private static string reportPath;

        public static void InitReport(AppConfigModel config)
        {

            string projectRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\"));
            reportPath = Path.Combine(projectRoot, config.Report.Path);
            Directory.CreateDirectory(Path.GetDirectoryName(reportPath));



            var sparkReporter = new ExtentSparkReporter(reportPath);
            sparkReporter.Config.DocumentTitle = config.Report.Title;
            sparkReporter.Config.ReportName = config.Report.Name;


            extentReport = new ExtentReports();
            extentReport.AttachReporter(sparkReporter);
        }

        public static void FlushReport()
        {
            extentReport.Flush();
        }

        public static void CreateTest(string testName)
        {
            extentTest = extentReport.CreateTest(testName);
        }

        public static void LogPass(string message, string screenshotPath = null)
        {
            if (!string.IsNullOrEmpty(screenshotPath))
                extentTest.Pass(message).AddScreenCaptureFromPath(screenshotPath);
            else
                extentTest.Pass(message);
        }

        public static void LogFail(string message, string screenshotPath = null)
        {
            if (!string.IsNullOrEmpty(screenshotPath))
                extentTest.Fail(message).AddScreenCaptureFromPath(screenshotPath);
            else
                extentTest.Fail(message);
        }
    }
}
