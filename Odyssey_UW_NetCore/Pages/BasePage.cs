using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using System;

namespace Odyssey_UW.Pages
{
    class BasePage
    {
        protected IWebDriver driver;
        protected WebDriverWait wait;
        protected IJavaScriptExecutor js;
        protected DefaultWait<IWebDriver> fluentWait;

        public BasePage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        }

        private IJavaScriptExecutor setJSExecutor()
        {
            return js = (IJavaScriptExecutor)driver;
        }

        IWebElement elemByXPath(string name) => driver.FindElement(By.XPath($"//label[text()='{name}']"));

        //public IIWebElement GetElemXPath(string name)
        //{
        //   //if(name.Contains(" "))
        //   // {
        //   //     XPath.Replace(" ", "+");
        //   // }
        //    return elemByXPath(name);
        //}

        protected void ExpectVisibility(By locator)
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));
        }

        protected string GetCurrentUrl()
        {
            return driver.Url;
        }

        protected IWebElement FluentWait(string xpathLocator)
        {
            fluentWait = new DefaultWait<IWebDriver>(driver);
            fluentWait.Timeout = TimeSpan.FromSeconds(10);
            fluentWait.PollingInterval = TimeSpan.FromSeconds(2);
            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            return fluentWait.Until(x => x.FindElement(By.XPath(xpathLocator)));
        }

        protected void ExpectInvisibility(By locator)
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(locator));
        }

        protected void ExpectElementToBeClickable(By locator)
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator));
        }

        protected void SearchValues(By[] locator, string[] value, By spinner)
        {
            for (int i = 0; i < locator.Length; i++)
            {
                ExpectVisibility(locator[i]);
                driver.FindElement(locator[i]).SendKeys(value[i] + Keys.Enter);
                ExpectInvisibility(spinner);
            }
        }

        protected bool IsEnabled(By locator)
        {
            return driver.FindElement(locator).Enabled;
        }

        protected void scrollAllDown()
        {
            setJSExecutor();
            js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
        }

        protected void scrollToElement(By elementToView)
        {
            setJSExecutor();
            var element = driver.FindElement(elementToView);
            js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }


        protected void unlockProgram()
        {
            By unlockBtn = By.XPath("//button[contains(@class, 'btn alert-action')]");
            driver.FindElement(unlockBtn).Click();
            ExpectInvisibility(unlockBtn);
        }

        protected void clearInput(By locator)
        {
            Actions actions = new Actions(driver);
            actions.Click(driver.FindElement(locator))
                .KeyDown(Keys.Control)
                .SendKeys("a")
                .KeyUp(Keys.Control)
                .SendKeys(Keys.Backspace)
                .Build()
                .Perform();
        }

    }
}
