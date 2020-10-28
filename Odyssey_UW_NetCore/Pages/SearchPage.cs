using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Odyssey_UW_NetCore;
using Odyssey_UW_NetCore.Utils;
using System;

namespace Odyssey_UW.Pages
{
    class SearchPage : BasePage
    {
        private By newProgramBtn = By.XPath("//button[@routerlink='/underwriting/view/0/summary']");
        private By headerSearchPage = By.XPath("//h2[contains(text(), 'Treaty Programs')]");
        private By searchbox = By.XPath("//input[@type='search']");
        private By searchButton = By.CssSelector(".btn-primary");
        private By nextPageBtn = By.XPath("//clr-icon[@shape='angle right']");
        private By inputPageNumber = By.XPath("//input[contains(@class, 'pagination-current')]");
        private By totalPages = By.XPath("//span[@aria-label='Total Pages']");
        private By noProgramsFound = By.XPath("//div[text()='No programs found.']");
        private By firstRecentProgram = By.XPath("//div[@class='recent_item']");
        private By searchTable = By.XPath("//div[@class='datagrid-inner-wrapper']");

        private By searchBranch = By.XPath("//label[text()=' All Branches ']");
        private By searchUWYear = By.XPath("//label[text()=' All UW Year ']");
        private By searchBusUnit = By.XPath("//label[text()=' All Business Units ']");
        private By searchStatus = By.XPath("//label[text()=' All Status ']");
        private By previousPageBtn = By.XPath("//clr-icon[@class='previous-page']");
        NewProgramPage newProgramPage;
        ProgramEditionPage editionPage;
        private By nextPageButton = By.CssSelector(".pagination-next");
        //List<IIWebElement> table_rows = driver.findElements(By.XPath("//div[@role='grid']/clr-dg-row"));


        public SearchPage(IWebDriver driver) : base(driver)
        {
        }


        public bool IsDisplayedSearchPage()
        {
            ExpectVisibility(headerSearchPage);
            IWebElement headerSearch = driver.FindElement(headerSearchPage);
            if (headerSearch.Displayed)
            {
                return true;
            }
            return false;
        }

        public NewProgramPage ClickNewProgramButton()
        {
            ExpectVisibility(newProgramBtn);
            driver.FindElement(newProgramBtn).Click();
            NewProgramPage newProgramPage = new NewProgramPage(driver);
            if (newProgramPage.IsDisplayedNewProgramPage())
            {
                return newProgramPage;
            }
            else
            {
                Assert.Fail("The New Program Page isn't displayed");
                return null;
            }
        }

        private int getTotalPages()
        {
           ExpectVisibility(totalPages);
           var number = driver.FindElement(totalPages).Text;
           int converted = Convert.ToInt32(number);
           return converted;
        }

        private void searchByName(string programName)
        {
            ExpectVisibility(searchbox);
            driver.FindElement(searchbox).SendKeys(programName + Keys.Enter);
            ExpectElementToBeClickable(searchButton);
        }

        public void validateCreationByRecentPrograms(string nameProgram)
        {
            ExpectVisibility(firstRecentProgram);
            scrollToElement(firstRecentProgram);
            IWebElement recentProgramCreated = driver.FindElement(firstRecentProgram);
            string nameRecentProgram = driver.FindElement(By.XPath("//div[@class='recent_item']/p")).Text;
            //if (nameRecentProgram.Trim().Equals(nameProgram))
            //{
            recentProgramCreated.Click();
            editionPage = new ProgramEditionPage(driver);
            Assert.IsTrue(editionPage.IsDisplayedEditionPage(nameProgram));
            //}
            //else
            //{
            //    Assert.Fail("The recent Program: {0} doesn't match with the name of the Program created: {1}", nameRecentProgram.Trim(), nameProgram);
            //}
        }

        public void searchProgramCreated(string nameProgram)
        {
            searchByName(nameProgram);
            IList<IWebElement> table_rows = driver.FindElements(By.XPath("//div[@role='grid']/clr-dg-row"));

            if (table_rows.Count > 10)


                for (int i = 1; i < getTotalPages(); i++)
                {
                    for (int j = 1; j <= table_rows.Count; j++)
                    {
                        string colName, colTreaty, colEffDate, colUW, colBusUnit, colBranch;

                        colName = driver.FindElement(By.XPath("//clr-dg-row[" + i + "]//div[@class='datagrid-scrolling-cells']/clr-dg-cell[2]")).Text;
                        colTreaty = driver.FindElement(By.XPath("//clr-dg-row[" + i + "]//div[@class='datagrid-scrolling-cells']/clr-dg-cell[3]")).Text;
                        colEffDate = driver.FindElement(By.XPath("//clr-dg-row[" + i + "]//div[@class='datagrid-scrolling-cells']/clr-dg-cell[4]")).Text;
                        colUW = driver.FindElement(By.XPath("//clr-dg-row[" + i + "]//div[@class='datagrid-scrolling-cells']/clr-dg-cell[5]")).Text;
                        colBusUnit = driver.FindElement(By.XPath("//clr-dg-row[" + i + "]//div[@class='datagrid-scrolling-cells']/clr-dg-cell[6]")).Text;
                        colBranch = driver.FindElement(By.XPath("//clr-dg-row[" + i + "]//div[@class='datagrid-scrolling-cells']/clr-dg-cell[7]")).Text;

                        if (driver.FindElement(noProgramsFound).Displayed)
                        {
                            Assert.Fail("The Program {0} was not created", nameProgram);
                        }
                        else if (colName.Equals(newProgramPage.GetProgramName()) && colTreaty.Equals(newProgramPage.GetTreatyTypeSelected()) && colEffDate.Equals(newProgramPage.GetEffDateDisplayed())
                            && colUW.Equals(newProgramPage.GetUWSelected()) && colBusUnit.Equals(newProgramPage.GetBunitSelected()) && colBranch.Equals(newProgramPage.GetBranchSelected()))
                        {
                            int programId = int.Parse(driver.FindElement(By.XPath("//clr-dg-row[" + i + "]//div[@class='datagrid-scrolling-cells']/clr-dg-cell[1]")).Text);
                            table_rows[i].Click();
                            break;
                        }
                        //nextPageButton.Click();
                    }


                }
        }

        private void enterPageNumber(int pageNum)
        {
            ExpectVisibility(inputPageNumber);
            clearInput(inputPageNumber);
            IWebElement element = driver.FindElement(inputPageNumber);
            element.SendKeys(pageNum + Keys.Enter);
        }


        public void checkAllPrograms()
        {
            if (IsDisplayedSearchPage())
            {
                int currentPage = 1;
                IWebElement program;
                string colName;
                int totalPages = getTotalPages();

                for (int i = 49; i < totalPages; i++)
                {
                    IList<IWebElement> table_rows = driver.FindElements(By.XPath("//div[@role='grid']/clr-dg-row"));
                    for (int j = 1; j <= table_rows.Count; j++)
                    {
                        ExpectVisibility(searchTable);
                        //int pageNumber = int.Parse(driver.FindElement(By.XPath("//input[@aria-label='Current Page']")).GetAttribute("value"));
                        program = driver.FindElement(By.XPath($"//div[@role='grid']/clr-dg-row[{j}]"));
                        colName = driver.FindElement(By.XPath($"//clr-dg-row[{j}]//div[@class='datagrid-scrolling-cells']/clr-dg-cell[2]")).Text;
                        program.Click();
                        editionPage = new ProgramEditionPage(driver);
                        Assert.That(editionPage.IsDisplayedEditionPage(colName));
                        //if (!editionPage.exitProgram())
                        //{
                            HttpUtils http = new HttpUtils(GetCurrentUrl());
                            ProgramObj affectedProgram = new ProgramObj()
                            {
                                //Id = http.GetResponseDataId(),
                                ProgName = colName,
                                ResponseCode = http.GetResponseCode(),
                                Data = http.GetResponseData()
                            };

                            http.writeFile(affectedProgram, "AffectedPrograms");
                        //}
                    }
                    currentPage++;
                    enterPageNumber(currentPage);
                }

            }
        }






    }





}

