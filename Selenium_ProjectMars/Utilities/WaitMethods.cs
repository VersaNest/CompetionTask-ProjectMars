using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;


namespace Selenium_ProjectMars.Utilities
{
    public class WaitMethods
    {
        protected IWebDriver driver;
        public WaitMethods(IWebDriver driver)
        {
            this.driver = driver;
        }
        protected WebDriverWait Wait(int timeoutInSeconds = 30)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
        }

        public IWebElement WaitUntilVisible(By locator, int timeoutInSeconds = 15)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            wait.PollingInterval = TimeSpan.FromMilliseconds(500);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));
        }

        public IWebElement WaitUntilClickable(By locator, int timeoutInSeconds = 30)
        {
            return Wait(timeoutInSeconds).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator));
        }

        public IReadOnlyCollection<IWebElement> WaitUntilVisibleAll(By locator, int timeoutInSeconds = 10)
        {
            try
            {
                return new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds)).Until(drv =>
                {
                    var elements = drv.FindElements(locator);
                    return elements;
                });
            }
            catch (WebDriverTimeoutException)
            {
                return new List<IWebElement>();
            }
        }

        public void WaitUntilInvisible(By locator, int timeoutInSeconds = 15)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            wait.Until(driver =>
            {
                try
                {
                    var element = driver.FindElement(locator);
                    return !element.Displayed;
                }
                catch (NoSuchElementException)
                {
                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    return true;
                }
            });
        }
    }
}
