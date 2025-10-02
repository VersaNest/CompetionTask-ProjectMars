using NUnit.Framework;
using OpenQA.Selenium;
using Selenium_ProjectMars.Models;
using Selenium_ProjectMars.Pages;
using Selenium_ProjectMars.Utilities;
using SeleniumProjectMars.Utilities;


namespace Selenium_ProjectMars.Tests
{
    public class BaseTest
    {
        protected static IWebDriver driver;
        protected AppConfigModel config;

       
        public IWebDriver Driver => driver;
        public AppConfigModel Config => config;

        [OneTimeSetUp]
        public void GlobalSetup()
        {
            config = JsonDataReader.LoadJson<AppConfigModel>("TestData\\AppConfig.json");
            ExtentReportManager.InitReport(config);
        }

        [SetUp]
        public void testSetUp()
        {
            
            config = JsonDataReader.LoadJson<AppConfigModel>("TestData\\AppConfig.json");

            driver = DriverSetup.StartDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(config.Url);

            ExtentReportManager.CreateTest(TestContext.CurrentContext.Test.Name);
        }

        [TearDown]
        public void testCleanUp()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var testName = TestContext.CurrentContext.Test.Name;

            try
            {
                string screenshotPath = ScreenshotSetup.TakeScreenshot(driver, testName);

                if (status == NUnit.Framework.Interfaces.TestStatus.Failed)
                {
                   
                   
                    ExtentReportManager.LogFail("Test Failed", screenshotPath);
                }
                else
                {
                    ExtentReportManager.LogPass("Test Passed", screenshotPath);
                }
            }
            catch (Exception ex)
            {
                TestContext.Progress.WriteLine($"Error in TearDown: {ex.Message}");
            }
            finally
            {
                var certificationPage = new CertificationPage(driver);
                var educationPage = new EducationPage(driver);

                certificationPage.ClickCertificationsTab();
                certificationPage.WaitUntilCertificationsTabContentVisible();
                certificationPage.DeleteAllCertifications();

                educationPage.ClickEducationTab();
                educationPage.WaitUntilEducationTabContentVisible();
                educationPage.DeleteAllEducations();


                DriverSetup.QuitDriver();
                driver.Dispose();
            }
        }

        [OneTimeTearDown]
        public void GlobalCleanup()
        {
           
            ExtentReportManager.FlushReport();
        }
    }
}
