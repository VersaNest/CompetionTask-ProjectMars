
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
              
        private IWebDriver driver;        
        private LoginPage loginPage;
        private BasePage basePage;
        private CertificationPage certificationPage;
        private BaseTest baseTest;


        private List<LoginModel> loginData;
     
        private List<CertificationModel> addCertData;
        private List<CertificationModel> cancelCertData;
        private List<CertificationModel> deleteCertData;
        private List<CertificationModel> dupCertData;
        private List<CertificationModel> emptyCertData;
        private List<CertificationModel> updateCertData;
        private List<CertificationModel> newCertData;
        private List<CertificationModel> dupCertDetails;

        private string expectedMessage;
        private string displayedMessage;

       

       

        [SetUp]

        public void TestSetUp()
        {


            baseTest = new BaseTest();
            baseTest.TestSetup(); // start driver and create test
            driver = baseTest.driver;

            loginPage = new LoginPage(driver);
            basePage = new BasePage(driver);
            certificationPage = new CertificationPage(driver);

            loginData = JsonDataReader.LoadJson<List<LoginModel>>("TestData\\Login_Data\\ValidLoginData.json");
            

            loginPage.SignFromMain();
            var validLogin = loginData[0];
            loginPage.UserLogin(validLogin.Email, validLogin.Password);
        }

        
       
       

        [Test]
        /*
         * This test verifies 
         * more than 10 certifications can be added 
         * where names contain combination of letters, symbols and numbers 
         */
        public void VerifyAddCertificaton()
        {
            addCertData = JsonDataReader.LoadJson<List<CertificationModel>>("TestData\\Certifications_Data\\AddCertificationsData.json");
            foreach (var newCert in addCertData)
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
            cancelCertData = JsonDataReader.LoadJson<List<CertificationModel>>("TestData\\Certifications_Data\\CancelCertificationsData.json");
            certificationPage.AddNewCertification(cancelCertData[0].Certificate_Award,
                cancelCertData[0].Certified_From, cancelCertData[0].Year);
            basePage.CloseConfirmationMessage();

            certificationPage.EnterNewCertificationDetails(cancelCertData[1].Certificate_Award,
                cancelCertData[1].Certified_From, cancelCertData[1].Year);
            basePage.cancelSelection();

            string displayedCertName = basePage.VerifyDetailsDisplayed(1);
            Assert.That(displayedCertName, Is.Not.EqualTo(cancelCertData[1].Certificate_Award),
                "Certification has been added even after cancelling");

        }

        //Test case to verify user able to error message for empty field submission
        [Test]

        public void VerifyEmptyFieldValidation()
        {
            emptyCertData = JsonDataReader.LoadJson<List<CertificationModel>>("TestData\\Certifications_Data\\EmptyCertificationsFieldData.json");

            foreach (var newCert in emptyCertData)
            {
                certificationPage.AddNewCertification(newCert.Certificate_Award, newCert.Certified_From, newCert.Year);
                displayedMessage = basePage.RetreiveMessage();
                expectedMessage = "Please enter Certification Name, Certification From and Certification Year";


                Assert.That(displayedMessage, Is.EqualTo(expectedMessage),
                    "Confirmation Message not displayed as expected");
                basePage.CloseConfirmationMessage();
                basePage.cancelSelection();

            }
            
           
           
        }

        //Test case to verify user able to delete any certification from profile
        [Test]
        public void VerifyDeleteCertification()
        {
        
            deleteCertData = JsonDataReader.LoadJson<List<CertificationModel>>("TestData\\Certifications_Data\\DeleteCertificationsData.json");
            
            expectedMessage = $"{deleteCertData[1].Certificate_Award} has been deleted from your certification";

            foreach (var newCert in deleteCertData)
            {
                certificationPage.AddNewCertification(newCert.Certificate_Award, newCert.Certified_From, newCert.Year);
                basePage.CloseConfirmationMessage();

            }

            displayedMessage = certificationPage.DeleteCertification(deleteCertData[1].Certificate_Award);
            Assert.That(displayedMessage, Is.EqualTo(expectedMessage), "Certification not found to delete");
            basePage.CloseConfirmationMessage();
        }

        //Test case to verify user able to update any certification from profile
        [Test]
        public void VerifyUpdateCertification()
        {
           updateCertData = JsonDataReader.LoadJson<List<CertificationModel>>("TestData\\Certifications_Data\\UpdateCertificationsData.json");
           newCertData = JsonDataReader.LoadJson<List<CertificationModel>>("TestData\\Certifications_Data\\NewCertificationDetails.json");
            foreach (var newCert in updateCertData)
            {
                certificationPage.AddNewCertification(newCert.Certificate_Award, newCert.Certified_From, newCert.Year);
                basePage.CloseConfirmationMessage();

            }

            displayedMessage = certificationPage.UpdateCertification(updateCertData[2].Certificate_Award,
                newCertData[0].Certificate_Award, newCertData[0].Certified_From,
                newCertData[0].Year);

            expectedMessage = $"{newCertData[0].Certificate_Award} has been updated to your certification";
            Assert.That(displayedMessage, Is.EqualTo(expectedMessage),
              "Confirmation Message not displayed as expected");

        

            string updatedCert = certificationPage.GetUpdatedCertDetails(newCertData[0].Certificate_Award);

           
            Assert.That(updatedCert, Is.EqualTo(newCertData[0].Certificate_Award), 
                "Certification not updated as required");


            certificationPage.WaitUntilCertificationsTabContentVisible();
        }

        // Test to verify error message while trying to add duplicate certification details
        [Test]

        public void VerifyAddExistingCertification()
        {
            dupCertData = JsonDataReader.LoadJson<List<CertificationModel>>("TestData\\Certifications_Data\\DupCheckCertificationsData.json");
            dupCertDetails = JsonDataReader.LoadJson<List<CertificationModel>>("TestData\\Certifications_Data\\DupCertificationDetails.json");

            foreach (var newCert in dupCertData)
            {
                certificationPage.AddNewCertification(newCert.Certificate_Award, newCert.Certified_From, newCert.Year);
                basePage.CloseConfirmationMessage();

            }

            foreach (var newCert in dupCertDetails)
            {
                string duplicateStatus = certificationPage.CheckDuplicateCert(newCert.Certificate_Award,
                newCert.Certified_From, newCert.Year);

                certificationPage.AddNewCertification(newCert.Certificate_Award, newCert.Certified_From, newCert.Year);

                displayedMessage = basePage.RetreiveMessage();
                

                if (duplicateStatus == "All three duplicate")
                {
                    expectedMessage = "This information is already exist.";

                    Assert.That(displayedMessage, Is.EqualTo(expectedMessage), "Confirmation Message not displayed as expected");
                    basePage.cancelSelection();
                }
                else if (duplicateStatus == "First two duplicate")
                {
                    expectedMessage = "Duplicated data";

                    Assert.That(displayedMessage, Is.EqualTo(expectedMessage), "Confirmation Message not displayed as expected");
                    basePage.cancelSelection();
                }
                else
                {
                    expectedMessage = $"{newCert.Certificate_Award} has been added to your certification";

                    Assert.That(displayedMessage, Is.EqualTo(expectedMessage), "Confirmation Message not displayed as expected");
                }

                basePage.CloseConfirmationMessage();

            }
            
        }


        [TearDown]
        public void Cleanup()
        {
            baseTest.TestCleanUp();
        }

    }
}
