using NUnit.Framework;
using OpenQA.Selenium;

namespace Odyssey_UW.Pages
{
    class LoginPage : BasePage
    {
        private By userName = By.Id("okta-signin-username");
        private By passWord = By.Id("okta-signin-password");
        private By loginButton = By.Id("okta-signin-submit");
        private By uWBtn = By.XPath("//a[text()='Underwriting']");
        private By loginSuccessful = By.XPath("//h1[contains(text(),'Welcome')]");
        private SearchPage searchPage;


        public LoginPage(IWebDriver driver) : base(driver)
        {
        }

        public void RegisterUser(string user, string pass)
        {
            ExpectVisibility(userName);
            driver.FindElement(userName).SendKeys(user);
            driver.FindElement(passWord).SendKeys(pass);
            driver.FindElement(loginButton).Click();
        }

        public SearchPage EnterUWPage()
        {
            ExpectVisibility(loginSuccessful);
            IWebElement message = driver.FindElement(loginSuccessful);
            if (message.Displayed)
            {
                Assert.AreEqual("Welcome to the Phoenix Reinsurance System", message.Text);
                driver.FindElement(uWBtn).Click();
                searchPage = new SearchPage(driver);
            }
            return searchPage;
        }
    }
}
