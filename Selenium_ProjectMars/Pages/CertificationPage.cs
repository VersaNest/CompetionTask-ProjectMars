using OpenQA.Selenium;

using Selenium_ProjectMars.Utilities;


namespace Selenium_ProjectMars.Pages
{

    public class CertificationPage
    {
        private IWebDriver driver;
        private readonly WaitMethods waitObj;
        private BasePage basePage;
        public CertificationPage(IWebDriver driver)
        {
            this.driver = driver;
            waitObj = new WaitMethods(driver);
            basePage = new BasePage(driver);
        }

        public void ClickCertificationsTab()
        {
            basePage.ClickTheTab(By.XPath("//a[@data-tab='fourth']"));
       
        }

        public void EnterCertificationsSection(string certName, string certFrom, string certYear)
        {
            basePage.enterDetails(By.XPath("//input[@placeholder='Certificate or Award']"), certName);
            basePage.enterDetails(By.XPath("//input[@placeholder='Certified From (e.g. Adobe)']"), certFrom);
            basePage.selectDropDownValue(By.XPath("//select[@name='certificationYear']"), certYear);

        }
        public void EnterNewCertificationDetails(string certName, string certFrom, string certYear)
        {

            ClickCertificationsTab();
            WaitUntilCertificationsTabContentVisible();

            basePage.ClickButton(By.XPath("(//div[contains(@class,'ui teal button')][normalize-space()='Add New'])[4]"));

            EnterCertificationsSection(certName, certFrom, certYear);

           
          
        }
        public void AddNewCertification(string certName, string certFrom, string certYear)
        {
            

            EnterNewCertificationDetails(certName, certFrom, certYear);

            basePage.ClickButton(By.XPath("//input[@value='Add']"));
           

        }

     
        public void WaitUntilCertificationsTabContentVisible()
        {
            basePage.WaitUntilTabContentVisible(By.XPath("//div[@data-tab='fourth' and contains(@class,'active')]"));
            
        }


        public void ActivateCertificationsTab()
        {
            ClickCertificationsTab();

            WaitUntilCertificationsTabContentVisible();
        }


        public List<IWebElement> GetCertificationRows()
        {
            
            return basePage.GetRows(By.XPath("//div[@data-tab='fourth']//table/tbody/tr"));
        }

        public string DeleteCertification(string delCertName)
        {
            ActivateCertificationsTab();
            var rows = GetCertificationRows();

            for (int i = 0; i < rows.Count; i++)
            {
                IWebElement certificationName = rows[i].FindElement(By.XPath("./td[1]"));

                if (certificationName.Text.Equals(delCertName, StringComparison.OrdinalIgnoreCase))
                {

                    basePage.clickRowIcon(rows[i], By.XPath("./td[last()]//i[contains(@class,'remove')]"));
                  
                
                    return basePage.RetreiveMessage();
                }
            }
            return "Failed";
        }

        public string UpdateCertification(string oldCertName, string newCertName, string newCertFrom, string newYear)
        {
            ActivateCertificationsTab();

            var rows = GetCertificationRows();


            for (int i = 0; i < rows.Count; i++)
            {
                IWebElement certName = rows[i].FindElement(By.XPath("./td[1]"));

                if (certName.Text.Equals(oldCertName, StringComparison.OrdinalIgnoreCase))
                {

                    basePage.clickRowIcon(rows[i], By.XPath("./td[4]//i[contains(@class,'outline write')]"));
                   

                    EnterCertificationsSection(newCertName, newCertFrom, newYear);

                    basePage.ClickButton(By.XPath("//div[@data-tab='fourth']//input[@value='Update']"));

                    return basePage.RetreiveMessage();
                  

                   
                }
            }

            return "Failed";
        }

        public string GetUpdatedCertDetails(string newCertName)
        {
            basePage.CloseConfirmationMessage();
            var updatedRows = GetCertificationRows();

            for (int j = 0; j < updatedRows.Count; j++)
            {
                IWebElement updatedCertName = updatedRows[j].FindElement(By.XPath("./td[1]"));
                if (updatedCertName.Text.Equals(newCertName, StringComparison.OrdinalIgnoreCase))
                {
                    return updatedCertName.Text;
                }
            }
            return "Failed";
        }

        public string CheckDuplicateCert(string certName, string certFrom, string certYear)
        {

 

            var rows = GetCertificationRows();

            for (int i = 0; i < rows.Count; i++)
            {
                IWebElement dataName = rows[i].FindElement(By.XPath("./td[1]"));

                if (dataName.Text.Equals(certName, StringComparison.OrdinalIgnoreCase))
                {
                    IWebElement dataFrom = rows[i].FindElement(By.XPath("./td[2]"));
                    IWebElement dataYear = rows[i].FindElement(By.XPath("./td[3]"));

                    if (dataFrom.Text.Equals(certFrom, StringComparison.OrdinalIgnoreCase))
                    {
                        if (dataYear.Text.Equals(certYear, StringComparison.OrdinalIgnoreCase))
                            return "All three duplicate";
                        else
                            return "First two duplicate";
                    }
                    else
                        return "Original";

                }
            }
            return "Original";
        }
       

        public void DeleteAllCertifications()
        {
            ActivateCertificationsTab();
            basePage.DeleteAllRows(By.XPath("//div[@data-tab='fourth' and contains(@class,'active')]//table/tbody/tr"));
        }

       

        

    }
}
