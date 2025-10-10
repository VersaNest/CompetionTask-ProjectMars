using NUnit.Framework;
using OpenQA.Selenium;
using Selenium_ProjectMars.Models;
using Selenium_ProjectMars.Pages;
using Selenium_ProjectMars.Utilities;
using System.Collections.Generic;

namespace Selenium_ProjectMars.Tests
{
    [TestFixture]
    public class LoginTests
    {
        private BaseTest baseTest;
        private IWebDriver driver;
        private LoginPage loginPage;
        private BasePage basePage;
        private List<LoginModel> validLoginData;
        private List<LoginModel> invalidLoginData;

        [SetUp]
        public void Setup()
        {
            baseTest = new BaseTest();
            baseTest.TestSetup(); // start driver and create test
            driver = baseTest.driver;

            basePage = new BasePage(driver);
            loginPage = new LoginPage(driver);

            
        }

     

        [Test]
        public void VerifySuccessfulLogin()
        {

            validLoginData = JsonDataReader.LoadJson<List<LoginModel>>("TestData\\Login_Data\\ValidLoginData.json");

            loginPage.SignFromMain();
            Assert.That(loginPage.LoginModalVisible(), Is.True, "Login modal did not appear!");

            
            loginPage.UserLogin(validLoginData[0].Email, validLoginData[0].Password);

            string loggedUser = loginPage.VerifyUser();
            Assert.That(loggedUser.Contains(validLoginData[0].FirstName), "Correct user not logged in");
        }

        [Test]
        public void VerifyUnsuccessfulLogin()
        {
            loginPage.SignFromMain();
            Assert.That(loginPage.LoginModalVisible(), Is.True, "Login modal did not appear!");



            validLoginData = JsonDataReader.LoadJson<List<LoginModel>>("TestData\\Login_Data\\ValidLoginData.json");

            invalidLoginData = JsonDataReader.LoadJson<List<LoginModel>>("TestData\\Login_Data\\InvalidLoginData.json");

            loginPage.UserLogin(invalidLoginData[0].Email, invalidLoginData[0].Password);

            loginPage.EnterInvalidVerification(validLoginData[0].Email);

            string displayedMessage = basePage.RetreiveMessage();
            Assert.That(displayedMessage, Is.EqualTo("Email Verification Sent"), "Confirmation message incorrect");

            loginPage.SignFromMain();
            loginPage.UserLogin(validLoginData[0].Email, validLoginData[0].Password);
        }

        [TearDown]
        public void Cleanup()
        {
            baseTest.TestCleanUp();
        }
    }
}
