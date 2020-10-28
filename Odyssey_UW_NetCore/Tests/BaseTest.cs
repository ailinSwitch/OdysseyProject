using NUnit.Framework;
using NUnit.Framework.Internal;
using Odyssey_UW.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager.DriverConfigs.Impl;

namespace Odyssey_UW.Tests
{
    [TestFixture]
    public class BaseTest
    {
        protected IWebDriver driver;
        private LoginPage loginPage;


        public IWebDriver getDriver()
        {
            return driver;
        }

        [OneTimeSetUp]
        public void LoginApplication()
        {
            new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig()); 
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("start-maximized");
            driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl("http://localhost:4200");
            loginPage = new LoginPage(driver);
            loginPage.RegisterUser("AilinGarcia", "Agonzalez123");
            loginPage.EnterUWPage();
        }


        //[OneTimeTearDown]
        //public void RecordFailure()
        //{
        //    if (TestContext.CurrentContext.Result.Outcome != ResultState.Success)
        //    {
        //        Screenshot screenShot = ((ITakesScreenshot)driver).GetScreenshot();
        //        string timeStamp = DateTime.Today.ToString("yyyy/MM/dd");
        //        screenShot.SaveAsFile(@"D:\VS\Odyssey_UW\Odyssey_UW\Screenshots\" + TestContext.CurrentContext.Test.Name + TestContext.CurrentContext.Result.Outcome.Status+ " " +timeStamp+ ".png", ScreenshotImageFormat.Png);
        //        //driver.Quit();
        //    }
        //}


    }
}
