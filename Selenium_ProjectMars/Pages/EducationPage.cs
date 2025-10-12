using OpenQA.Selenium;

using Selenium_ProjectMars.Utilities;


namespace Selenium_ProjectMars.Pages
{
    public class EducationPage
    {
        private IWebDriver driver;
        private readonly WaitMethods waitObj;
        private BasePage basePage;

        public EducationPage(IWebDriver driver)
        {
            this.driver = driver;
            waitObj = new WaitMethods(driver);
            basePage = new BasePage(driver);
        }

        public void ClickEducationTab()
        {
            basePage.ClickTheTab(By.XPath("//a[@data-tab='third']"));
       
        }

        public void EnterEducationsSection(string uniName, string eduCountry, string eduTitle, string eduDegree, string eduYear)
        {
            basePage.enterDetails(By.XPath("//input[@placeholder='College/University Name']"), uniName);
            basePage.selectDropDownValue(By.XPath("//select[@name='country']"), eduCountry);
            basePage.selectDropDownValue(By.XPath("//select[@name='title']"), eduTitle);
            basePage.enterDetails(By.XPath("//input[@placeholder='Degree']"), eduDegree);
            basePage.selectDropDownValue(By.XPath("//select[@name='yearOfGraduation']"), eduYear);
        }

        public void EnterEducationDetails(string uniName, string eduCountry, string eduTitle, string eduDegree, string eduYear)
        {

            ClickEducationTab();

            WaitUntilEducationTabContentVisible();

            basePage.ClickButton(By.XPath("(//div[contains(@class,'ui teal button')][normalize-space()='Add New'])[3]"));

            EnterEducationsSection(uniName, eduCountry, eduTitle, eduDegree, eduYear);

        }



        public void AddNewEducation(string uniName, string eduCountry, string eduTitle, string eduDegree, string eduYear)
        {


            EnterEducationDetails(uniName, eduCountry, eduTitle, eduDegree, eduYear);
            basePage.ClickButton(By.XPath("//input[@value='Add']"));

        }

       


        public void WaitUntilEducationTabContentVisible()
        {

            basePage.WaitUntilTabContentVisible(By.XPath("//div[@data-tab='third' and contains(@class,'active')]"));
           
        }


        public void ActivateEducationTab()
        {
            ClickEducationTab();

            WaitUntilEducationTabContentVisible();
        }

        public List<IWebElement> GetEducationRows()
        {

            return basePage.GetRows(By.XPath("//div[@data-tab='third']//table/tbody/tr"));
        }

        public string DeleteEducation(string delEduName)
        {
            ActivateEducationTab();
            var rows = GetEducationRows();

            for (int i = 0; i < rows.Count; i++)
            {
                IWebElement educationName = rows[i].FindElement(By.XPath("./td[4]"));

                if (educationName.Text.Equals(delEduName, StringComparison.OrdinalIgnoreCase))
                {
                    basePage.clickRowIcon(rows[i], By.XPath("./td[last()]//i[contains(@class,'remove')]"));

                    return basePage.RetreiveMessage();
                }
            }
            return "Failed";
        }


        public string UpdateEducation(string oldDegree, string newUniName, string newEduCountry, string newEduTitle, string newEduDegree, string newEduYear)
        {
            ActivateEducationTab();
            var rows = GetEducationRows();


            for (int i = 0; i < rows.Count; i++)
            {
                IWebElement degreeName = rows[i].FindElement(By.XPath("./td[4]"));

                if (degreeName.Text.Equals(oldDegree, StringComparison.OrdinalIgnoreCase))
                {

                    basePage.clickRowIcon(rows[i], By.XPath("./td[6]//i[contains(@class,'outline write')]"));

                    EnterEducationsSection(newUniName, newEduCountry, newEduTitle, newEduDegree, newEduYear);

                    basePage.ClickButton(By.XPath("//div[@data-tab='third']//input[@value='Update']"));

                    return basePage.RetreiveMessage();

                }
            }

            return "Failed";
        }

        public string GetUpdatedEduDetails(string newEduDegree)
        {
            basePage.CloseConfirmationMessage();
            var updatedRows = GetEducationRows();


            for (int j = 0; j < updatedRows.Count; j++)
            {
                IWebElement updatedDegreeName = updatedRows[j].FindElement(By.XPath("./td[4]"));
                if (updatedDegreeName.Text.Equals(newEduDegree, StringComparison.OrdinalIgnoreCase))
                {
                    return updatedDegreeName.Text;
                }
            }

            return "Failed";

        }

        public string CheckDuplicateEdu(string uniName, string eduCountry, string eduTitle, string eduDegree, string eduYear)
        {

            var rows = GetEducationRows();

            for (int i = 0; i < rows.Count; i++)
            {
                IWebElement dataName = rows[i].FindElement(By.XPath("./td[4]"));

                if (dataName.Text.Equals(eduDegree, StringComparison.OrdinalIgnoreCase))
                {
                    IWebElement dataCountry = rows[i].FindElement(By.XPath("./td[1]"));
                    IWebElement dataUniversity = rows[i].FindElement(By.XPath("./td[2]"));
                    IWebElement dataTitle = rows[i].FindElement(By.XPath("./td[3]"));
                    IWebElement dataYear = rows[i].FindElement(By.XPath("./td[5]"));

                    if (dataCountry.Text.Equals(eduCountry, StringComparison.OrdinalIgnoreCase) &&
                        dataUniversity.Text.Equals(uniName, StringComparison.OrdinalIgnoreCase) &&
                        dataTitle.Text.Equals(eduTitle, StringComparison.OrdinalIgnoreCase))
                    {
                        if (dataYear.Text.Equals(eduYear, StringComparison.OrdinalIgnoreCase))
                            return "All duplicate";
                        else
                            return "First four duplicate";
                    }
                    else
                        return "Original";

                }
            }
            return "Original";
        }

     


        public void DeleteAllEducations()
        {


            ActivateEducationTab();
            basePage.DeleteAllRows(By.XPath("//div[@data-tab='third' and contains(@class,'active')]//table/tbody/tr"));
        }

      

       


    }
}
