
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Odyssey_UW.Pages
{
    class NewProgramPage : BasePage
    {
        private By newProgramHeader = By.XPath("//h2[contains(text(), 'New Program')]");
        private By programName = By.XPath("//input[@formcontrolname='name']");
        private string[] xPathSelectDrop = { "//select[@formcontrolname='assumingCompanyId']", "//select[@formcontrolname='branchId']", "//select[@formcontrolname='businessUnitId']" };
        private string[] xPathTypeAheadDrop = { "//ng-select[@formcontrolname='treatyTypeId']//input", "//ng-select[@formcontrolname='underwriterId']//input" };
        private By container = By.XPath("//div[contains(@class, 'ng-option')]");
        private By effDateBtn = By.XPath("(//button[contains(@title, 'Toggle datepicker')])[1]");
        By effectiveDateInput = By.XPath("(//input[contains(@class, 'input-date')])[1]");
        By expirationDateInput = By.XPath("(//input[contains(@class, 'input-date')])[2]");
        By cedantInput = By.XPath("//uwrt-searchable-input[1]//input[@type='text']");
        By brokerInput = By.XPath("//uwrt-searchable-input[2]//input[@type='text']");
        By searchSpinner = By.XPath("//i[@class='fa fa-refresh fa-spin ng-star-inserted']");
        By createBtn = By.XPath("//div/button[3]");
        By toastMessage = By.XPath("//div[contains(@class,'toast-message')]");
        By xolTypeBtn = By.XPath("//label[.='Excess']");

        //    By quickSearchModal = By.XPath("//input[@placeholder='Quick Search']");
        By cedantSearchClearBtn = By.XPath("//uwrt-searchable-input[1]//clr-icon[@role='none']");
        By brokerSearchClearBtn = By.XPath("//uwrt-searchable-input[2]//clr-icon[@role='none']");
        By totalPages = By.XPath("//div[contains(@class, 'pagination-list')]/span");
        By leadUWInput = By.XPath("//ng-select[@formcontrolname='underwriterId']//input");
        By treatyTypeInput = By.XPath("//ng-select[@formcontrolname='treatyTypeId']//input");
        By prorataTypeBtn = By.XPath("//label[.='Pro rata']");


        private string nameEntered, leadUWEntered, monthString;
        int day, year, month;
        private IWebElement cedantField, brokerField, bUnitSelect, branchSelect;
        private SelectElement select;
        private SearchPage searchPage;


        public NewProgramPage(IWebDriver driver) : base(driver)
        {
        }

        public bool IsDisplayedNewProgramPage()
        {
            ExpectVisibility(newProgramHeader);
            IWebElement headerNewProgram = driver.FindElement(newProgramHeader);
            if (headerNewProgram.Displayed)
            {
                Assert.AreEqual("New Program", headerNewProgram.Text, "The actual header doesn't match with the expected");
                return true;
            }
            return false;
        }

        public void SetProgramName(string name)
        {
            nameEntered = name;
            ExpectVisibility(programName);
            driver.FindElement(programName).SendKeys(name);
        }

        public string GetProgramName() => nameEntered;

        public void SetCedantAndBroker(string cedant, string broker)
        {
            By[] searchLocators = { brokerInput, cedantInput };
            string[] values = { cedant, broker };
            SearchValues(searchLocators, values, searchSpinner);
        }

        public bool IsBrokerEnabled()
        {
            return IsEnabled(brokerInput);
        }

        public bool IsCedantEnabled() => IsEnabled(cedantInput);


        public string GetCedantSelected()
        {
            cedantField = driver.FindElement(cedantInput);
            return cedantField.GetAttribute("value");
        }

        public string GetBrokerSelected()
        {
            brokerField = driver.FindElement(brokerInput);
            return brokerField.GetAttribute("value");
        }

        public void SetContractType(string type)
        {
            if (type.Equals("XOL"))
            {
                driver.FindElement(xolTypeBtn).Click();
            }
        }

        public void SetSelectDpd(string[] values)
        {
            IWebElement[] webElem = new IWebElement[xPathSelectDrop.Length];

            for (int i = 0; i < xPathSelectDrop.Length; i++)
            {
                webElem[i] = FluentWait(xPathSelectDrop[i]);
                SelectElement select = new SelectElement(webElem[i]);
                select.SelectByText(values[i]);
            }
        }

        public void SetTypeAheadDpd(string[] values)
        {
            IWebElement[] inputFields = new IWebElement[xPathTypeAheadDrop.Length];

            for (int i = 0; i < xPathTypeAheadDrop.Length; i++)
            {
                inputFields[i] = driver.FindElement(By.XPath(xPathTypeAheadDrop[i]));
                inputFields[i].SendKeys(values[i]);
                var allValues = driver.FindElements(container);

                foreach (IWebElement val in allValues)
                {
                    if (val.Text.Contains(values[i]))
                    {
                        val.Click();
                        break;
                    }
                }
            }
        }

        private void ValidateToastMessage(string expectedMessage)
        {
            ExpectVisibility(toastMessage);
            string actualMessage = driver.FindElement(toastMessage).Text;
            Assert.AreEqual(expectedMessage, actualMessage);
        }
        public void ClickCreateBtn()
        {
            string programCreated = "Program was successfully created";
            string programNotCreated = "Program cannot be created until all required fields are entered";
            driver.FindElement(createBtn).Click();
            searchPage = new SearchPage(driver);

            if (searchPage.IsDisplayedSearchPage())
            {
                ValidateToastMessage(programCreated);
                driver.Navigate().Refresh();
            }
            else
            {
                ValidateToastMessage(programNotCreated);
                Assert.Fail("Some required field is missing. The Program can't be created");
            }
        }

        private int MonthToInt(string month)
        {
            return DateTime.ParseExact(month, "MMMM", null).Month;
        }

        private int CompareDateWithActual(int year, int month, int day)
        {
            DateTime actualDate = DateTime.Now;
            DateTime dateToCompare = new DateTime(year, month, day);
            return actualDate.CompareTo(dateToCompare);
        }

        private void SetDay()
        {
            IList<IWebElement> allDays = driver.FindElements(By.XPath("//clr-calendar/table/tr[contains(@class, 'ng-star-inserted')]/td"));
            foreach (IWebElement d in allDays)
            {
                string actualDay = d.Text;
                if (actualDay.Equals(day.ToString()))
                {
                    d.Click();
                    break;
                }
            }
        }

        private void SetMonth()
        {
            IWebElement monthSelector = driver.FindElement(By.XPath("//div[@class='calendar-pickers']/button[@class='calendar-btn monthpicker-trigger']"));
            string actualMonth = monthSelector.Text;

            if (!actualMonth.Contains(monthString))
            {
                driver.FindElement(By.XPath("//div[@class='calendar-pickers']/button[@class='calendar-btn monthpicker-trigger']")).Click();
                IList<IWebElement> allMonths = driver.FindElements(By.XPath("//clr-datepicker-view-manager/clr-monthpicker/button"));
                foreach (IWebElement m in allMonths)
                {
                    string thisMonth = m.Text;
                    if (thisMonth.Equals(monthString))
                    {
                        m.Click();
                        break;
                    }
                }
            }
        }

        private void SetYear()
        {
            IWebElement previousDecadeBtn, nextDecadeBtn;
            int difference, numberClicks, index, yearIndex, currentYear;
            DateTime currentDate = DateTime.Today;
            currentYear = currentDate.Year;

            driver.FindElement(By.XPath("//div[@class='calendar-pickers']/button[@class='calendar-btn yearpicker-trigger']")).Click();

            previousDecadeBtn = driver.FindElement(By.XPath("//button[@class='calendar-btn switcher'][1]"));
            nextDecadeBtn = driver.FindElement(By.XPath("//button[@class='calendar-btn switcher'][3]"));
            difference = year - currentYear;
            numberClicks = difference / 10;
            index = 11;
            yearIndex = 1;

            if (year > currentYear)
            {
                yearIndex = yearIndex + difference % 10;
                for (int i = 0; i < numberClicks; i++)
                {
                    nextDecadeBtn.Click();
                    i++;
                }
                driver.FindElement(By.XPath("//div[@class='years']/button[" + yearIndex + "]")).Click();
            }
            else
            {
                difference *= -1;
                yearIndex = index - (difference % 10);
                previousDecadeBtn.Click();
                for (int i = 0; i < numberClicks; i++)
                {
                    previousDecadeBtn.Click();
                    i++;
                }
                driver.FindElement(By.XPath("//div[@class='years']/button[" + yearIndex + "]")).Click();
            }
        }

        public void SetEffDate(string month, int day, int year)
        {
            this.day = day;
            this.year = year;
            this.month = MonthToInt(month);
            monthString = month;

            int datesResult = CompareDateWithActual(year, MonthToInt(month), day);
            ExpectVisibility(effDateBtn);
            driver.FindElement(effDateBtn).Click();
            ExpectVisibility(By.CssSelector("clr-datepicker-view-manager"));

            if (datesResult == 0)
            {
                driver.FindElement(By.XPath("//button[contains(@class, 'is-today')]")).Click();
            }
            else
            {
                DateTime currentDate = DateTime.Today;
                int currentYear = currentDate.Year;
                SetMonth();
                if (currentYear == year)
                {
                    SetDay();
                }
                else
                {
                    SetYear();
                    SetDay();
                }
            }
        }

        public string GetEffDateDisplayed() => driver.FindElement(effectiveDateInput).GetAttribute("value");
        public string GetExpDateDisplayed() => driver.FindElement(expirationDateInput).GetAttribute("value");

        private int MonthMaxLenght(int month) => DateTime.DaysInMonth(year, month);

        public void VerifyExpDateDisplayed()
        {
            DateTime expDateExpected;
            if (day == 1)
            {
                int previousMonth = month - 1;
                int fixedDay = MonthMaxLenght(previousMonth);
                expDateExpected = new DateTime(year + 1, previousMonth, fixedDay);
            }
            else
            {
                expDateExpected = new DateTime(year + 1, month, day - 1);
            }

            DateTime expDateActual = DateTime.ParseExact(GetExpDateDisplayed(), "MM/dd/yyyy", CultureInfo.InvariantCulture);

            Assert.IsTrue(expDateExpected.CompareTo(expDateActual) == 0, "The actual Expiration Date ({0}) is not the expected ({1})", GetExpDateDisplayed(), expDateExpected);
        }

        public string GetTreatyTypeSelected() => driver.FindElement(treatyTypeInput).Text;
        public string GetUWSelected() => driver.FindElement(leadUWInput).Text;
        public string GetBranchSelected()
        {
            branchSelect = driver.FindElement(By.XPath("//select[@formcontrolname='branchId']"));
            select = new SelectElement(branchSelect);
            return select.SelectedOption.Text;
        }
        public string GetBunitSelected()
        {
            bUnitSelect = driver.FindElement(By.XPath("//select[@formcontrolname='businessUnitId']"));
            select = new SelectElement(bUnitSelect);
            return select.SelectedOption.Text;
        }



    }
}
