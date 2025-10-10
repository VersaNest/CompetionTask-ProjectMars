using OpenQA.Selenium;

using OpenQA.Selenium.Support.UI;
using Selenium_ProjectMars.Utilities;


namespace Selenium_ProjectMars.Pages
{
    public class BasePage
    {
        private IWebDriver driver;
        private readonly WaitMethods waitObj;

        public BasePage(IWebDriver driver)
        {
            this.driver = driver;
            waitObj = new WaitMethods(driver);
        }

        public string RetreiveMessage()
        {
            IWebElement displayedMessage = waitObj.WaitUntilVisible(By.XPath("//div[@class='ns-box-inner']"));

            return displayedMessage.Text;
        }

        public void CloseConfirmationMessage()
        {
            By closeButton = By.CssSelector("a.ns-close");
            waitObj.WaitUntilClickable(closeButton).Click();
            waitObj.WaitUntilInvisible(closeButton);
        }

        public string VerifyDetailsDisplayed(int columnIndex)
        {
            IWebElement lastRow = waitObj.WaitUntilVisible(By.XPath($"//table/tbody[last()]/tr[last()]/td[{columnIndex}]"));
            return lastRow.Text;

        }

        public void ClickTheTab(By tabLocator)
        {
            IWebElement tabInput = waitObj.WaitUntilClickable(tabLocator);
            tabInput.Click();


        }

        public void enterDetails(By elementLocator, string newValue)
        {
            IWebElement elementInput = waitObj.WaitUntilVisible(elementLocator);
            elementInput.Clear();
            elementInput.SendKeys(newValue);
        }

        public void selectDropDownValue(By elementLocator, string newValue)
        {

            IWebElement elementDropdown = waitObj.WaitUntilVisible(elementLocator);
            SelectElement selectElement = new SelectElement(elementDropdown);

            if (!string.IsNullOrWhiteSpace(newValue))
            {
                selectElement.SelectByText(newValue);
            }
        }

       
        public void WaitUntilTabContentVisible(By tableLocator)
        {
            try
            {
                waitObj.WaitUntilVisible(tableLocator);

            }
            catch (WebDriverTimeoutException)
            {
                throw new Exception("Tab content did not appear within timeout.");
            }
        }

     

        public void ClickButton(By buttonLocator)
        {
            IWebElement requiredButton = waitObj.WaitUntilClickable(buttonLocator);
            requiredButton.Click();
        }

        public void clickRowIcon(IWebElement selectedRow, By iconLocator)
        {
            var iconSelected = selectedRow.FindElement(iconLocator);
            iconSelected.Click();
        }

        public List<IWebElement> GetRows(By rowLocator)
        {
            var rows = waitObj.WaitUntilVisibleAll(rowLocator);
            return rows.ToList();
        }

        public void cancelSelection()
        {
            ClickButton(By.XPath("//input[@value='Cancel']"));
        }
        public void DeleteAllRows(By tableLocator)
        {
            IReadOnlyCollection<IWebElement> rows;

            try
            {
                rows = driver.FindElements(tableLocator);
            }
            catch (NoSuchElementException)
            {
                return;
            }

            while (rows.Count > 0)
            {
                try
                {
                    var deleteIcon = rows.First().FindElement(By.XPath(".//i[contains(@class,'remove icon')]"));
                    deleteIcon.Click();

                    try
                    {
                        CloseConfirmationMessage();
                     
                    }
                    catch (WebDriverTimeoutException)
                    {

                    }

                    rows = driver.FindElements(tableLocator);
                }
                catch (StaleElementReferenceException)
                {

                    rows = driver.FindElements(tableLocator);
                }
            }
        }



    }
}
