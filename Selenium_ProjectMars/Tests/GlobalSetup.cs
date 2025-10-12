using NUnit.Framework;
using Selenium_ProjectMars.Models;
using Selenium_ProjectMars.Utilities;
using SeleniumProjectMars.Utilities;

namespace Selenium_ProjectMars.Tests
{
    [SetUpFixture]
    public class GlobalSetup
    {
        public static AppConfigModel config;

        [OneTimeSetUp]
        public void RunBeforeAllTests()
        {
            
            config = JsonDataReader.LoadJson<AppConfigModel>("TestData\\AppConfig.json");
            ExtentReportManager.InitReport(config);
        }

        [OneTimeTearDown]
        public void RunAfterAllTests()
        {
            ExtentReportManager.FlushReport();
        }
    }
}
