# ProjectMars

ProjectMars is a web-based application where users can create their account and add their education, certification etc.  
Only authorised users can access the application with valid Email and Password.

---

## Features Implemented
- User Authentication: Login with valid credentials, Verification for invalid login.
- Profile Management: Add, update, and delete functionalities for education and certifications.
- Screenshot capture for all test cases (pass and fail)
- ExtentReports integration for detailed HTML reports (HTML test report is generated in Reports/TestReport.html)
- Page Object Model (POM) to maintain reusable page methods
- Test cleanup ensures no data passed on to future test executions

---

## Technologies Used
- Reqnroll (Behavior Driven Development - BDD)
- Selenium
- C#

---

## Project Structure
`````
├─ Tests/ # Test classes (BaseTest, CertificationTests, etc.)
├─ Pages/ # Page Object Model classes (LoginPage, CertificationPage, EducationPage, BasePage)
├─ Models/ # Data models (AppConfigModel, LoginModel, CertificationModel, etc.)
├─ Utilities/ # Helpers (DriverSetup, ScreenshotSetup, ExtentReportManager, JsonDataReader)
├─ TestData/ # JSON files with test data
├─ Reports/ # HTML test reports (generated after test run)
`````
---

## Prerequisites
- **Visual Studio 2022** or higher
- **.NET 8.0 SDK**
- **Google Chrome** browser
- NuGet packages:
  - Selenium.WebDriver
  - Selenium.Support
  - NUnit
  - NUnit3TestAdapter
  - AventStack.ExtentReports
  - Newtonsoft.Json
  - DotNetSeleniumExtras.WaitHelpers

---

## Instructions for Running the Tests
1. Clone the project repository: `http://git.mvp.studio/qa-examples/ta_mars_docker.git` and follow the steps given.
2. Clone this repository.
3. Open **Visual Studio 2022** and install the required NuGet packages.
4. Build the solution and run the test cases.

---

## Notes
- `bin/` and `obj/` folders are excluded from the repository.
- Test cases for the features are added in the Excel file and uploaded.
