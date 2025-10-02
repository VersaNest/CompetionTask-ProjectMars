
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;


namespace Selenium_ProjectMars.Utilities
{
    public static class DriverSetup
    {
        private static IWebDriver driver;

        public static IWebDriver StartDriver() 
        {
            var options = new ChromeOptions();
            options.AddArgument("start-maximized");
            driver = new ChromeDriver(options);
            return driver;
        }


        public static void QuitDriver()
        {
            if (driver != null)
            {
                driver.Quit();   
                driver = null;
            }
        }
    }
}
