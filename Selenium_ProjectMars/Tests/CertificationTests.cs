
using NUnit.Framework;
using OpenQA.Selenium;
using Selenium_ProjectMars.Models;
using Selenium_ProjectMars.Pages;
using Selenium_ProjectMars.Utilities;
using SeleniumProjectMars.Utilities;


namespace Selenium_ProjectMars.Tests
{
    [TestFixture]
    public class CertificationTests
    {
        private BaseTest baseTest;       
        private IWebDriver driver;        
        private LoginPage loginPage;
        private BasePage basePage;
        private CertificationPage certificationPage;
        protected AppConfigModel config;

        private List<LoginModel> loginData;
        private List<CertificationModel> certificationData;
        private List<UpdatedCertificationModel> updatedCertificationData;

        private string expectedMessage;
        private string displayedMessage;

        int addCount = 0;

        [OneTimeSetUp]
        public void GlobalSetup()
        {
            baseTest = new BaseTest();


            config = JsonDataReader.LoadJson<AppConfigModel>("TestData\\AppConfig.json");
            ExtentReportManager.InitReport(config);
        }

        [SetUp]

        public void TestSetUp()
        {

            baseTest = new BaseTest();
            baseTest.testSetUp();
            driver = baseTest.Driver; 

            loginPage = new LoginPage(driver);
            basePage = new BasePage(driver);
            certificationPage = new CertificationPage(driver);

            loginData = JsonDataReader.LoadJson<List<LoginModel>>("TestData\\LoginData.json");
            certificationData = JsonDataReader.LoadJson<List<CertificationModel>>("TestData\\CertificationDetails.json");

            loginPage.SignFromMain();
            var validLogin = loginData[0];
            loginPage.UserLogin(validLogin.Email, validLogin.Password);
        }

        
        //This adds only 5 certifications to verify update, delete and duplicate certification functionalities
        public void AddFiveCertifications() 
        {
            foreach (var newCert in certificationData)
            {
                if (addCount >= 5) break;
                certificationPage.AddNewCertification(newCert.Certificate_Award, newCert.Certified_From, newCert.Year);
                basePage.CloseConfirmationMessage();
                addCount++;
            }
        }
       

        [Test]
        /*
         * This test verifies 
         * more than 10 certifications can be added 
         * where names contain combination of letters, symbols and numbers 
         */
        public void VerifyAddCertificaton()
        {

            foreach (var newCert in certificationData)
            {
                certificationPage.AddNewCertification(newCert.Certificate_Award, newCert.Certified_From, newCert.Year);

                expectedMessage = $"{newCert.Certificate_Award} has been added to your certification";
                displayedMessage = basePage.RetreiveMessage();
                Assert.That(displayedMessage, Is.EqualTo(expectedMessage), "Confirmation Message not displayed as expected");

                string displayedCertName = basePage.VerifyDetailsDisplayed(1);
                Assert.That(displayedCertName, Is.EqualTo(newCert.Certificate_Award), "Certification Name doesnot match");

                string displayedCertFrom = basePage.VerifyDetailsDisplayed(2);
                Assert.That(displayedCertFrom, Is.EqualTo(newCert.Certified_From), "Certification From doesnot match");

                string displayedCertYear = basePage.VerifyDetailsDisplayed(3);
                Assert.That(displayedCertYear, Is.EqualTo(newCert.Year), "Certification Year doesnot match");

                basePage.CloseConfirmationMessage();
            }
    
        }

        //Test to verify cancel button performs as expected and certification not added to profile
        [Test]
        public void VerifyCancelButton()
        {
            certificationPage.AddNewCertification(certificationData[0].Certificate_Award,
                certificationData[0].Certified_From, certificationData[0].Year);
            basePage.CloseConfirmationMessage();

            certificationPage.EnterNewCertificationDetails(certificationData[1].Certificate_Award,
                certificationData[1].Certified_From, certificationData[1].Year);
            basePage.ClickButton(By.XPath("//input[@value='Cancel']"));
            string displayedCertName = basePage.VerifyDetailsDisplayed(1);
            Assert.That(displayedCertName, Is.Not.EqualTo(certificationData[1].Certificate_Award),
                "Certification has been added even after cancelling");

        }

        /*This test case validates the screenshot feature for error messages, as application shows error message 
     * instead of confirmation when we leave the field blank while adding new certification*/
        [Test]

        public void VerifyEmptyFieldValidation()
        {
            certificationPage.AddNewCertification(certificationData[0].Certificate_Award,
                "", certificationData[0].Year);
            displayedMessage = basePage.RetreiveMessage();
            expectedMessage = $"{certificationData[0].Certificate_Award} has been added to your certification";
            

            Assert.That(displayedMessage, Is.EqualTo(expectedMessage), 
                "Confirmation Message not displayed as expected");
        }

        //Test case to verify user able to delete any certification from profile
        [Test]
        public void VerifyDeleteCertification()
        {
            AddFiveCertifications();

            expectedMessage = $"{certificationData[1].Certificate_Award} has been deleted from your certification";
            
            displayedMessage = certificationPage.DeleteCertification(certificationData[1].Certificate_Award);
            Assert.That(displayedMessage, Is.EqualTo(expectedMessage), "Certification not found to delete");
            basePage.CloseConfirmationMessage();
        }

        //Test case to verify user able to update any certification from profile
        [Test]
        public void VerifyUpdateCertification()
        {
            updatedCertificationData = JsonDataReader.LoadJson<List<UpdatedCertificationModel>>("TestData\\UpdatedCertificationDetails.json");

            AddFiveCertifications();

            displayedMessage = certificationPage.UpdateCertification(certificationData[1].Certificate_Award,
                updatedCertificationData[0].Certificate_Award, updatedCertificationData[0].Certified_From,
                updatedCertificationData[0].Year);

            expectedMessage = $"{updatedCertificationData[0].Certificate_Award} has been updated to your certification";
            Assert.That(displayedMessage, Is.EqualTo(expectedMessage),
              "Confirmation Message not displayed as expected");

            string updatedCert = certificationPage.GetUpdatedCertDetails(updatedCertificationData[0].Certificate_Award);

           
            Assert.That(updatedCert, Is.EqualTo(updatedCertificationData[0].Certificate_Award), 
                "Certification not updated as required");
            certificationPage.WaitUntilCertificationsTabContentVisible();
        }

        // Test to verify error message while trying to add duplicate certification details
        [Test]

        public void VerifyAddExistingCertification()
        {
            AddFiveCertifications();

            string duplicateStatus = certificationPage.CheckDuplicateCert(certificationData[1].Certificate_Award, 
                certificationData[1].Certified_From, certificationData[1].Year);

            certificationPage.AddNewCertification(certificationData[1].Certificate_Award,
                certificationData[1].Certified_From, certificationData[1].Year);
            
            
            displayedMessage = basePage.RetreiveMessage();

            if (duplicateStatus == "All three duplicate")
            {
                expectedMessage = "This information is already exist.";

                Assert.That(displayedMessage, Is.EqualTo(expectedMessage), "Confirmation Message not displayed as expected");
            }
            else if (duplicateStatus == "First two duplicate")
            {
                expectedMessage = "Duplicated data";

                Assert.That(displayedMessage, Is.EqualTo(expectedMessage), "Confirmation Message not displayed as expected");
            }
            else
            {
                expectedMessage = $"{certificationData[1].Certificate_Award} has been added to your certification";
               
                Assert.That(displayedMessage, Is.EqualTo(expectedMessage), "Confirmation Message not displayed as expected");
            }

            basePage.CloseConfirmationMessage();
     
        }

        [TearDown]
        public void TestDataCleanUp()
        {
            
            
            baseTest.testCleanUp();

        }

        [OneTimeTearDown]
        public void GlobalCleanup()
        {
         
            ExtentReportManager.FlushReport();
        }

    }
}
