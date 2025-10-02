using NUnit.Framework;
using OpenQA.Selenium;
using Selenium_ProjectMars.Models;
using Selenium_ProjectMars.Pages;
using Selenium_ProjectMars.Utilities;
using SeleniumProjectMars.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium_ProjectMars.Tests
{
    [TestFixture]
    public class EducationTests
    {
        private BaseTest baseTest;
        private IWebDriver driver;
        private LoginPage loginPage;
        private BasePage basePage;
        private EducationPage educationPage;
        protected AppConfigModel config;

        private List<LoginModel> loginData;
        private List<EducationModel> educationData;
        private List<UpdatedEducationModel> updatedEducationData;

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
            educationPage = new EducationPage(driver);

            loginData = JsonDataReader.LoadJson<List<LoginModel>>("TestData\\LoginData.json");
            educationData = JsonDataReader.LoadJson<List<EducationModel>>("TestData\\EducationDetails.json");

            loginPage.SignFromMain();
            var validLogin = loginData[0];
            loginPage.UserLogin(validLogin.Email, validLogin.Password);
        }

        //This adds only 5 educations from the json file to verify update, delete and duplicate education functionalities
        public void AddFiveEducations()
        {
            foreach (var newEdu in educationData)
            {
                if (addCount >= 5) break;
                educationPage.AddNewEducation(newEdu.College_University_Name, newEdu.Country, newEdu.Title, newEdu.Degree, newEdu.Year_of_graduation);
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
        public void VerifyAddEducation()
        {

            foreach (var newEdu in educationData)
            {
                educationPage.AddNewEducation(newEdu.College_University_Name, newEdu.Country, newEdu.Title, newEdu.Degree, newEdu.Year_of_graduation);

                expectedMessage = "Education has been added";
                displayedMessage = basePage.RetreiveMessage();
                Assert.That(displayedMessage, Is.EqualTo(expectedMessage), "Confirmation Message not displayed as expected");

                string displayedCountry = basePage.VerifyDetailsDisplayed(1);
                Assert.That(displayedCountry, Is.EqualTo(newEdu.Country), "Country name doesnot match");

                string displayedUniversity = basePage.VerifyDetailsDisplayed(2);
                Assert.That(displayedUniversity, Is.EqualTo(newEdu.College_University_Name), "College/University Name doesnot match");

                string displayedTitle = basePage.VerifyDetailsDisplayed(3);
                Assert.That(displayedTitle, Is.EqualTo(newEdu.Title), "Title doesnot match");

                string displayedDegree = basePage.VerifyDetailsDisplayed(4);
                Assert.That(displayedDegree, Is.EqualTo(newEdu.Degree), "Degree doesnot match");

                string displayedYear = basePage.VerifyDetailsDisplayed(5);
                Assert.That(displayedYear, Is.EqualTo(newEdu.Year_of_graduation), "Graduation Year doesnot match");

                basePage.CloseConfirmationMessage();
            }

        }

        //Test to verify cancel button performs as expected and education not added to profile
        [Test]
        public void VerifyCancelButton()
        {
            educationPage.AddNewEducation(educationData[2].College_University_Name, educationData[2].Country,
                educationData[2].Title, educationData[2].Degree, educationData[2].Year_of_graduation);
            basePage.CloseConfirmationMessage();

            educationPage.EnterEducationDetails(educationData[1].College_University_Name, educationData[1].Country,
                educationData[1].Title, educationData[1].Degree, educationData[1].Year_of_graduation);
            basePage.ClickButton(By.XPath("//input[@value='Cancel']"));

            string displayedDegreeName = basePage.VerifyDetailsDisplayed(4);
            Assert.That(displayedDegreeName, Is.Not.EqualTo(educationData[1].Degree),
                "Education degree has been added even after cancelling");

        }


        /*This test case validates the screenshot feature for error messages, as application shows error message 
         * instead of confirmation when we leave the field blank while adding new education*/

        [Test]

        public void VerifyEmptyFieldValidation()
        {
            educationPage.AddNewEducation("", "",
                educationData[1].Title, educationData[1].Degree, educationData[1].Year_of_graduation);
            displayedMessage = basePage.RetreiveMessage();
            expectedMessage = "Education has been added";

            Assert.That(displayedMessage, Is.EqualTo(expectedMessage),
                "Confirmation Message not displayed as expected");
        }

        //Test case to verify user able to delete any education from profile

        [Test]
        public void VerifyDeleteEducation()
        {
            AddFiveEducations();

            expectedMessage = "Education entry successfully removed";
            displayedMessage = educationPage.DeleteEducation(educationData[2].Degree);
            Assert.That(displayedMessage, Is.EqualTo(expectedMessage), "Education not found to delete");
            basePage.CloseConfirmationMessage();

        }

        //Test case to verify user able to update any education from profile

        [Test]
        public void VerifyUpdateEducation()
        {
            updatedEducationData = JsonDataReader.LoadJson<List<UpdatedEducationModel>>("TestData\\UpdatedEducationDetails.json");

            AddFiveEducations();

            displayedMessage = educationPage.UpdateEducation(educationData[1].Degree, updatedEducationData[0].College_University_Name,
                updatedEducationData[0].Country, updatedEducationData[0].Title, updatedEducationData[0].Degree, 
                updatedEducationData[0].Year_of_graduation);
            expectedMessage = "Education as been updated";
            Assert.That(displayedMessage, Is.EqualTo(expectedMessage), "Confirmation Message not displayed as expected");


            string updatedEdu = educationPage.GetUpdatedEduDetails(updatedEducationData[0].Degree);


            Assert.That(updatedEdu, Is.EqualTo(updatedEducationData[0].Degree), "Education not updated as required");
            educationPage.WaitUntilEducationTabContentVisible();
        }


        // Test to verify error message while trying to add duplicate education details

        [Test]

        public void VerifyAddExistingEducation()
        {
            AddFiveEducations();


            string duplicateStatus = educationPage.CheckDuplicateEdu(educationData[2].College_University_Name, 
                educationData[2].Country, educationData[2].Title, educationData[2].Degree, 
                educationData[2].Year_of_graduation);

            educationPage.AddNewEducation(educationData[2].College_University_Name,
                educationData[2].Country, educationData[2].Title, educationData[2].Degree,
                educationData[2].Year_of_graduation);


            displayedMessage = basePage.RetreiveMessage();

            if (duplicateStatus == "All duplicate")
            {
                expectedMessage = "This information is already exist.";

                Assert.That(displayedMessage, Is.EqualTo(expectedMessage), "Confirmation Message not displayed as expected");
            }
            else if (duplicateStatus == "First four duplicate")
            {
                expectedMessage = "Duplicated data";

                Assert.That(displayedMessage, Is.EqualTo(expectedMessage), "Confirmation Message not displayed as expected");
            }
            else
            {
                expectedMessage = "Education has been added";

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
