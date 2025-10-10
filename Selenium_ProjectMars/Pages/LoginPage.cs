
using OpenQA.Selenium;

using Selenium_ProjectMars.Utilities;


namespace Selenium_ProjectMars.Pages
{
    public class LoginPage
    {

        By loginModal = By.CssSelector("div.ui.tiny.modal.transition.visible.active");
     

        private BasePage basePage;
        private readonly By emailField = By.Name("email");
        private readonly By passwordField = By.Name("password");
        private readonly By loginButton = By.XPath("//button[contains(text(),'Login')]");

        private IWebDriver driver;
        private readonly WaitMethods waitObj;
        public LoginPage(IWebDriver driver)
        {
            this.driver = driver;
            waitObj = new WaitMethods(driver);

            basePage = new BasePage(driver);
        }

        public void SignFromMain()
        {
            basePage.ClickButton(By.XPath("//a[contains(text(),'Sign In')]"));
            
           
        }

       
        public bool LoginModalVisible(int timeoutInSeconds = 20)
        {

            try
            {
                waitObj.WaitUntilVisible(loginModal);
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public void UserLogin(string email, string password)
        {
            basePage.enterDetails(emailField, email);
            basePage.enterDetails(passwordField, password);
            basePage.ClickButton(loginButton);


        }


        public string VerifyUser()
        {
            var hiUserLocator = By.XPath("//span[contains(@class,'item ui dropdown link') and starts-with(text(),'Hi ')]");
            IWebElement hiUser = waitObj.WaitUntilVisible(hiUserLocator);
            return hiUser.Text;

        }

        public void GetEmailVerificationField(string userEmail)
        {

            basePage.CloseConfirmationMessage();
            basePage.enterDetails(emailField, userEmail);
          

        }
        public void EnterInvalidVerification(string userEmail)
        {
            GetEmailVerificationField(userEmail);
            basePage.ClickButton(By.XPath("//button[@id='submit-btn']"));
           
        }

        

    }
}
