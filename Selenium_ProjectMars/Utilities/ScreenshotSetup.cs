
using OpenQA.Selenium;

namespace SeleniumProjectMars.Utilities
{
    public static class ScreenshotSetup
    {
        public static string TakeScreenshot(IWebDriver driver, string testName)
        {
            try
            {
                ITakesScreenshot screenshotDriver = (ITakesScreenshot)driver;
                Screenshot screenshot = screenshotDriver.GetScreenshot();

                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string fileName = $"{testName}_{timestamp}.png";

                string screenshotsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots");

                if (!Directory.Exists(screenshotsDir))
                    Directory.CreateDirectory(screenshotsDir);

                string filePath = Path.Combine(screenshotsDir, fileName);

                screenshot.SaveAsFile(filePath);

                return filePath;
            }
            catch
            {
                return null;
            }
        }
    }
}
