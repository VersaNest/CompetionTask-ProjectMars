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
       
        private IWebDriver driver;
        private LoginPage loginPage;
        private BasePage basePage;
        private EducationPage educationPage;
        private BaseTest baseTest;


        private List<LoginModel> loginData;
      

        private List<EducationModel> addEduData;
        private List<EducationModel> cancelEduData;
        private List<EducationModel> deleteEduData;
        private List<EducationModel> dupEduData;
        private List<EducationModel> emptyEduData;
        private List<EducationModel> updateEduData;
        private List<EducationModel> newEduData;
        private List<EducationModel> dupEduDetails;

        private string expectedMessage;
        private string displayedMessage;

        int addCount = 0;

       

        [SetUp]

        public void TestSetUp()
        {
            baseTest = new BaseTest();
            baseTest.TestSetup(); // start driver and create test
            driver = baseTest.driver;

            loginPage = new LoginPage(driver);
            basePage = new BasePage(driver);
            educationPage = new EducationPage(driver);

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
        public void VerifyAddEducation()
        {
            addEduData = JsonDataReader.LoadJson<List<EducationModel>>("TestData\\Education_Data\\AddEducationsData.json");

            foreach (var newEdu in addEduData)
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

            cancelEduData = JsonDataReader.LoadJson<List<EducationModel>>("TestData\\Education_Data\\CancelEducationsData.json");

            educationPage.AddNewEducation(cancelEduData[0].College_University_Name, cancelEduData[0].Country,
                cancelEduData[0].Title, cancelEduData[0].Degree, cancelEduData[0].Year_of_graduation);
            basePage.CloseConfirmationMessage();

            educationPage.EnterEducationDetails(cancelEduData[1].College_University_Name, cancelEduData[1].Country,
                cancelEduData[1].Title, cancelEduData[1].Degree, cancelEduData[1].Year_of_graduation);
            basePage.ClickButton(By.XPath("//input[@value='Cancel']"));

            string displayedDegreeName = basePage.VerifyDetailsDisplayed(4);
            Assert.That(displayedDegreeName, Is.Not.EqualTo(cancelEduData[1].Degree),
                "Education degree has been added even after cancelling");

        }


        /*This test case validates the screenshot feature for error messages, as application shows error message 
         * instead of confirmation when we leave the field blank while adding new education*/

        [Test]

        public void VerifyEmptyFieldValidation()
        {

            emptyEduData = JsonDataReader.LoadJson<List<EducationModel>>("TestData\\Education_Data\\EmptyEducationsFieldData.json");

            foreach (var newEdu in emptyEduData)
            {
                educationPage.AddNewEducation(newEdu.College_University_Name, newEdu.Country,
                    newEdu.Title, newEdu.Degree, newEdu.Year_of_graduation);

                displayedMessage = basePage.RetreiveMessage();
                expectedMessage = "Please enter all the fields";

                Assert.That(displayedMessage, Is.EqualTo(expectedMessage),
                       "Confirmation Message not displayed as expected");
                basePage.CloseConfirmationMessage();
                basePage.cancelSelection();

            }
        }
         //Test case to verify user able to delete any education from profile

        [Test]
        public void VerifyDeleteEducation()
        {

            deleteEduData = JsonDataReader.LoadJson<List<EducationModel>>("TestData\\Education_Data\\DeleteEducationsData.json");

            expectedMessage = "Education entry successfully removed";

            foreach (var newEdu in deleteEduData)
            {
                educationPage.AddNewEducation(newEdu.College_University_Name, newEdu.Country,
                    newEdu.Title, newEdu.Degree, newEdu.Year_of_graduation);

         
                basePage.CloseConfirmationMessage();
        

            }

            displayedMessage = educationPage.DeleteEducation(deleteEduData[2].Degree);


            Assert.That(displayedMessage, Is.EqualTo(expectedMessage), "Education not found to delete");
            basePage.CloseConfirmationMessage();

        }

        //Test case to verify user able to update any education from profile

        [Test]
        public void VerifyUpdateEducation()
        {
            updateEduData = JsonDataReader.LoadJson<List<EducationModel>>("TestData\\Education_Data\\UpdateEducationsData.json");
            newEduData = JsonDataReader.LoadJson<List<EducationModel>>("TestData\\Education_Data\\NewEducationDetails.json");

            foreach (var newEdu in updateEduData)
            {
                educationPage.AddNewEducation(newEdu.College_University_Name, newEdu.Country,
                    newEdu.Title, newEdu.Degree, newEdu.Year_of_graduation);


                basePage.CloseConfirmationMessage();


            }


            displayedMessage = educationPage.UpdateEducation(updateEduData[1].Degree, newEduData[0].College_University_Name,
                newEduData[0].Country, newEduData[0].Title, newEduData[0].Degree,
                newEduData[0].Year_of_graduation);

            expectedMessage = "Education as been updated";

            Assert.That(displayedMessage, Is.EqualTo(expectedMessage), "Confirmation Message not displayed as expected");


            string updatedEdu = educationPage.GetUpdatedEduDetails(newEduData[0].Degree);


            Assert.That(updatedEdu, Is.EqualTo(newEduData[0].Degree), "Education not updated as required");
            educationPage.WaitUntilEducationTabContentVisible();
        }


        // Test to verify error message while trying to add duplicate education details

        [Test]

        public void VerifyAddExistingEducation()
        {
            dupEduData = JsonDataReader.LoadJson<List<EducationModel>>("TestData\\Education_Data\\DupCheckEducationsData.json");
            dupEduDetails = JsonDataReader.LoadJson<List<EducationModel>>("TestData\\Education_Data\\DupEducationDetails.json");

            foreach (var newEdu in dupEduData)
            {
                educationPage.AddNewEducation(newEdu.College_University_Name, newEdu.Country,
                    newEdu.Title, newEdu.Degree, newEdu.Year_of_graduation);


                basePage.CloseConfirmationMessage();


            }

            foreach (var newCert in dupEduDetails)
            {
                string duplicateStatus = educationPage.CheckDuplicateEdu(newCert.College_University_Name,
                newCert.Country, newCert.Title, newCert.Degree,
                newCert.Year_of_graduation);

                educationPage.AddNewEducation(newCert.College_University_Name,
                    newCert.Country, newCert.Title, newCert.Degree,
                    newCert.Year_of_graduation);


                displayedMessage = basePage.RetreiveMessage();

                if (duplicateStatus == "All duplicate")
                {
                    expectedMessage = "This information is already exist.";

                    Assert.That(displayedMessage, Is.EqualTo(expectedMessage), "Confirmation Message not displayed as expected");
                    basePage.cancelSelection();
                }
                else if (duplicateStatus == "First four duplicate")
                {
                    expectedMessage = "Duplicated data";

                    Assert.That(displayedMessage, Is.EqualTo(expectedMessage), "Confirmation Message not displayed as expected");
                    basePage.cancelSelection();
                }
                else
                {
                    expectedMessage = "Education has been added";

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
