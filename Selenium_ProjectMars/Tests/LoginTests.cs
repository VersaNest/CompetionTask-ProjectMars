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
    public class LoginTests
    {
        private BaseTest baseTest;       
        private IWebDriver driver;        
        private LoginPage loginPage;
        private List<LoginModel> loginData;
        private BasePage basePage;
        protected AppConfigModel config;

        [OneTimeSetUp]
        public void GlobalSetup()
        {
            baseTest = new BaseTest();


            config = JsonDataReader.LoadJson<AppConfigModel>("TestData\\AppConfig.json");
            ExtentReportManager.InitReport(config);
        }

        [SetUp]

        public void SetUp()
        {
            baseTest = new BaseTest();
            baseTest.testSetUp();
            driver = baseTest.Driver;

            basePage = new BasePage(driver);
            loginPage = new LoginPage(driver);

            loginData = JsonDataReader.LoadJson<List<LoginModel>>("TestData\\LoginData.json");
        }



        [Test]

        public void VerifySuccessfulLogin()
        {
            loginPage.SignFromMain();
            Assert.That(loginPage.LoginModalVisible(), Is.True, "Login modal did not appear!");

            var validLogin = loginData[0];
            loginPage.UserLogin(validLogin.Email, validLogin.Password);

            string loggedUser = loginPage.VerifyUser();
            Assert.That(loggedUser.Contains(validLogin.FirstName), "Correct user not logged in");
        }

        [Test]
        public void VerifyUnsuccessfulLogin()
        {
            loginPage.SignFromMain();
            Assert.That(loginPage.LoginModalVisible(), Is.True, "Login modal did not appear!");

            var validLogin = loginData[0];
            var invalidLogin = loginData[1];
            loginPage.UserLogin(invalidLogin.Email, invalidLogin.Password);

            loginPage.EnterInvalidVerification(validLogin.Email);

            String displayedMessage = basePage.RetreiveMessage();
            
            Assert.That(displayedMessage, Is.EqualTo("Email Verification Sent"), "Confirmation message for verification email not correct");

            loginPage.SignFromMain();
            loginPage.UserLogin(validLogin.Email, validLogin.Password);
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
