using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace Odyssey_UW.Pages
{
    class ProgramEditionPage : BasePage
    {
        private By previousPageBtn = By.XPath("//clr-icon[@class='previous-page']");
        private By layerGridTable = By.XPath("//table/tbody/tr");
        private By addLayerBtn = By.XPath("//button[text()='Add Layer ']");
        private By coveragePRBtn = By.XPath("//div/input[@value='option1']");
        private By coverageXOLBtn = By.XPath("//div/input[@value='option2']");
        private By specialTerminationBtn = By.XPath("//button[text()=' Special Termination ']");
        private By terrorismSanctionsBtn = By.XPath("//button[text()=' Terrorism/ Sanctions ']");
        private By riskTransferBtn = By.XPath("//button[text()=' Risk Transfer ']");
        private By activeBtn = By.XPath("//button[contains(@class, 'green')]");
        private By modalChangesPend = By.XPath("//h4[text()='Changes Pending']");
        private By modalErrorSaving = By.XPath("//h4[text()='Error Saving']");
        private By modalLeaveOption = By.XPath("//button[text()=' Leave ']");
        private By modalConfirmationMessage = By.XPath("//div[@class='modal-content']");
        private By unlockBtn = By.XPath("//button[contains(@class, 'btn alert-action')]");
        private int numOfSections, totalRows, riskLimit, occLimit, subjPremium, cession, authShare, sigShare;
        Dictionary<int, string> proRataInputFields = new Dictionary<int, string>()
        {
            {0, "riskLimit" },
            {1, "occLimit" },
            {2, "subjectPremium" },
            {3, "cession" },
            {4, "writtenShare" },
            {5, "signedShare" },
        };
        public enum CoverageType { ProRata, XOL };
        public enum ProRataFields { RiskLimit, OccLimit, SubjectPremium, Cession, AuthShare, SignedShare }
        public enum ProRataType { _100, Ceded };

        //Pro Rata fields
        private By inputRiskLimit = By.XPath("//input[@formcontrolname='riskLimit']");
        private By inputOccLimit = By.XPath("//input[@formcontrolname='occLimit']");
        private By inputSubjPremium = By.XPath("//input[@formcontrolname='subjectPremium']");
        private By inputCession = By.XPath("//input[@formcontrolname='cession']");
        //private By PartOfBasisBtn = By.XPath("//label[text()='Part Of']");
        private By partOfBasisBtn = By.XPath("//div/input[@formcontrolname='basis' and @ng-reflect-value='1']");
        private By ofBasisBtn = By.XPath("//label[text()='Of']");
        private By inputAuthShare = By.XPath("//input[@formcontrolname='writtenShare']");
        private By inputSignedShare = By.XPath("//input[@formcontrolname='signedShare']");
        private By calculatedOdyRiskLimit = By.XPath("//input[@formcontrolname='assumedRisk']");
        private By calculatedOdyOccLimit = By.XPath("//input[@formcontrolname='occRisk']");
        private By calculatedOdyPremium = By.XPath("//input[@formcontrolname='assumedPremium']");
        string[] xPathProRataFields = {"//input[@formcontrolname='riskLimit']", "//input[@formcontrolname='occLimit']", "//input[@formcontrolname='subjectPremium']", "//input[@formcontrolname='cession']",
            "//input[@formcontrolname='writtenShare']", "//input[@formcontrolname='signedShare']" };



        //XOL fields


        public ProgramEditionPage(IWebDriver driver) : base(driver)
        {
        }

        public bool IsDisplayedEditionPage(string programNameExpected)
        {
            By editionPageHeader = By.XPath("//h2[contains(text(), '" + programNameExpected + "')]");
            ExpectVisibility(editionPageHeader);
            IWebElement header = driver.FindElement(editionPageHeader);
            if (header.Displayed)
            {
                checkLock();
                Assert.That(header.Text.Trim().Contains(programNameExpected));
                //Assert.AreEqual(programNameExpected, header.Text.Trim(), "The actual header doesn't match with the expected");
                return true;
            }
            return false;
        }

        public bool exitProgram()
        {
            driver.FindElement(previousPageBtn).Click();
            SearchPage searchPage = new SearchPage(driver);
            if (IsChangesPending() || IsErrorSaving())
            {
                return false;
            }
            else
            {
                Assert.That(searchPage.IsDisplayedSearchPage());
                return true;
            }
        }

        public bool IsChangesPending()
        {
            try
            {
                if (driver.FindElement(modalChangesPend).Displayed)
                {
                    driver.FindElement(modalLeaveOption).Click();
                    return true;
                }
            }
            catch (NoSuchElementException)
            {
            }
            return false;
        }

        public bool IsErrorSaving()
        {
            try
            {
                if (driver.FindElement(modalErrorSaving).Displayed)
                {
                    driver.FindElement(modalLeaveOption).Click();
                    return true;
                }
            }
            catch (NoSuchElementException)
            {
            }
            return false;
        }

        //    private int totalOfLayers(CoverageType cov)
        //    {
        //        ExpectVisibility(layerGridTable);
        //        IList<IWebElement> layerGridTableRows = driver.FindElements(layerGridTable);

        //        if (haveSections())
        //        {
        //            displayAllSections();
        //        }

        //        if (cov == CoverageType.XOL && layerGridTableRows.Count > 1)
        //        {
        //            totalRows = driver.FindElements(By.XPath("//table/tbody/tr[contains(@class, 'totals')]")).Count;
        //            return layerGridTableRows.Count - totalRows;

        //        }
        //        else
        //        {
        //            return layerGridTableRows.Count;
        //        }

        //    }



        //    private bool haveSections()
        //    {
        //        try
        //        {
        //            By sectionArrows = By.XPath("//td/clr-icon[@dir='down' or @dir='right']");
        //            if (driver.FindElement(sectionArrows).Displayed)
        //            {
        //                numOfSections = driver.FindElements(sectionArrows).Count;
        //                return true;
        //            }
        //        }
        //        catch (NoSuchElementException)
        //        {
        //        }
        //        return false;
        //    }

        //    private void displayAllSections()
        //    {
        //        if (haveSections())
        //        {
        //            IWebElement sectionArrowFold = driver.FindElement(By.XPath("//td/clr-icon[@dir='right']"));
        //            for (int i = 1; i <= numOfSections; i++)
        //            {
        //                if (sectionArrowFold.Displayed)
        //                {
        //                    sectionArrowFold.Click();
        //                }
        //            }
        //        }
        //    }


        //    //public void DeleteLayer(int layerNumb)
        //    //{
        //    //    By deleteModal = By.XPath("//h4[text()='Delete Layer']");
        //    //    By yesBtnModal = By.XPath("//button[text() =' Yes ']");
        //    //    driver.FindElement(By.XPath("(//td/clr-icon[@shape ='trash'])[" + layerNumb + "]"));
        //    //}

        //    private void changeCoverage(CoverageType coverage)
        //    {
        //        try
        //        {
        //            if (driver.FindElement(coveragePRBtn).Selected && coverage == CoverageType.XOL)
        //            {
        //                driver.FindElement(coverageXOLBtn).Click();
        //                ExpectVisibility(modalConfirmationMessage);
        //                driver.FindElement(By.XPath("//button[text()=' Delete']"));
        //            }
        //        }
        //        catch (NoSuchElementException)
        //        {

        //            throw;
        //        }

        //    }

        //    public void AddLayer(CoverageType coverage, int layers)
        //    {
        //        ExpectVisibility(addLayerBtn);
        //        int currentLayerNumb = totalOfLayers(coverage);
        //        for (int i = 1; i < layers; i++)
        //        {
        //            driver.FindElement(addLayerBtn).Click();
        //        }
        //        Assert.IsTrue(currentLayerNumb + layers == totalOfLayers(coverage));
        //    }

        private void checkLock()
        {
            try
            {
                if (driver.FindElement(By.XPath("//div[contains(text(), 'You')]")).Displayed)
                {
                    unlockProgram();
                }
            }
            catch (NoSuchElementException)
            {
            }
        }

        private void unlockProgram()
        {
            driver.FindElement(unlockBtn).Click();
            ExpectInvisibility(unlockBtn);
        }

        //    private void checkCoverageType()
        //    {
        //        IWebElement pRCoverage = driver.FindElement(radioBtnCoverage);
        //        if (pRCoverage.Selected)
        //        {
        //            Assert.AreEqual("Prorata Structure", driver.FindElement(By.XPath("//div[contains(@class, 'heading')]")).Text);
        //        }
        //    }




        //    public void selectPRType(ProRataType type)
        //    {
        //        if (type == ProRataType._100)
        //        {
        //            Assert.IsTrue(driver.FindElement(By.XPath("//input[@formcontrolname='prorataStructureType' and @ng-reflect-value='0']")).Selected);
        //            IList<IWebElement> basisBtns = driver.FindElements(partOfBasisBtn);
        //        }
        //    }

        //    //public void EnterPRValues(string riskL, string occL, string subPremium, string cession, string authShare, string sigShare)
        //    //{
        //    //    scrollAllDown();
        //    //    IWebElement[] elements = new IWebElement[xPathProRataFields.Length];
        //    //    string[] inputNumbers = { riskL, occL, subPremium, cession, authShare, sigShare };
        //    //    for (int i = 0; i < xPathProRataFields.Length; i++)
        //    //    {
        //    //        elements[i] = driver.FindElement(By.XPath(xPathProRataFields[i]));
        //    //        elements[i].SendKeys(inputNumbers[i]);
        //    //    }
        //    //}

        //    public void EnterPRValues(int layerNumber, int riskL, int occL, int subPremium, int cession, int authShare, int sigShare)
        //    {
        //        riskLimit = riskL; occLimit = occL; subjPremium = subPremium; this.cession = cession; this.authShare = authShare; this.sigShare = sigShare;
        //        scrollAllDown();
        //        IWebElement riskLField = driver.FindElement(By.XPath("(//input[@formcontrolname='riskLimit'])[" + layerNumber + "])"));
        //        //IWebElement[] elements = new IWebElement[xPathProRataFields.Length];
        //        int[] inputNumbers = { riskL, occL, subPremium, cession, authShare, sigShare };
        //        for (int i = 0; i < xPathProRataFields.Length; i++)
        //        {
        //            elements[i] = driver.FindElement(By.XPath(xPathProRataFields[i]));
        //            elements[i].SendKeys(inputNumbers[i].ToString());
        //        }
        //    }

        //    private void setRiskLimitValue(int layerNumber)
        //    {
        //        IWebElement element = driver.FindElement(By.XPath("(//input[@formcontrolname='riskLimit'])[" + layerNumber + "])"));
        //        string value = element.GetAttribute("value");
        //        if (!string.IsNullOrEmpty(value))
        //        {
        //            riskLimit = int.Parse(element.Text);
        //            return riskLimit;
        //        }
        //    }



        //    public int GetRiskLimitValue(int layerNumber)
        //    {
        //        IWebElement element = driver.FindElement(By.XPath("(//input[@formcontrolname='riskLimit'])[" + layerNumber + "])"));
        //        string value = element.GetAttribute("value");
        //        if (!string.IsNullOrEmpty(value))
        //        {
        //            riskLimit = int.Parse(element.Text);
        //            return riskLimit;
        //        }

        //        return 0;
        //    }

        //    public int GetOccLimitValue(int layerNumber)
        //    {
        //        IWebElement element = driver.FindElement(By.XPath("(//input[@formcontrolname='occLimit'])[" + layerNumber + "])"));
        //        string value = element.GetAttribute("value");
        //        if (!string.IsNullOrEmpty(value))
        //        {
        //            occLimit = int.Parse(element.Text);
        //            return occLimit;
        //        }

        //        return 0;
        //    }

        //    public int GetSignedShareValue(int layerNumber)
        //    {
        //        IWebElement element = driver.FindElement(By.XPath("(//input[@formcontrolname='signedShare'])[" + layerNumber + "])"));
        //        string value = element.GetAttribute("value");
        //        if (!string.IsNullOrEmpty(value))
        //        {
        //            sigShare = int.Parse(element.Text);
        //            return sigShare;
        //        }

        //        return 0;
        //    }

        //    public int GetOdyRiskLimitValue(int layerNumber)
        //    {
        //        IWebElement element = driver.FindElement(By.XPath("(//input[@formcontrolname='assumedRisk'])[" + layerNumber + "])"));
        //        string value = element.GetAttribute("value");
        //        if (!string.IsNullOrEmpty(value))
        //        {
        //            sigShare = int.Parse(element.Text);
        //            return sigShare;
        //        }

        //        return 0;
        //    }

        //    public int GetFieldValue(int layerNumber, ProRataFields field)
        //    {
        //        switch (field)
        //        {
        //            case ProRataFields.RiskLimit:
        //               return int.Parse(driver.FindElement(By.XPath($"(//input[@formcontrolname='{proRataInputFields[0]}'])[" + layerNumber + "])")).Text);
        //                break;
        //            case ProRataFields.OccLimit:
        //                break;
        //            case ProRataFields.SubjectPremium:
        //                break;
        //            case ProRataFields.Cession:
        //                break;
        //            case ProRataFields.AuthShare:
        //                break;
        //            case ProRataFields.SignedShare:
        //                break;
        //            default:
        //                break;
        //        }
        //        if(field==ProRataFields.RiskLimit)
        //        {
        //            fieldName = fieldName.ToLower();
        //        }
        //    }

        //    private int odyRiskLimitResult(int layerNumber)
        //    {
        //        int valueRiskLimit = GetRiskLimitValue(layerNumber);
        //        valueRiskLimit
        //        int valueSignedShare = GetSignedShareValue(layerNumber);

        //        if (valueRiskLimit != 0 && valueSignedShare != 0)
        //        {
        //            return valueRiskLimit * valueSignedShare / 100;
        //            //return Math.Round(result);
        //        }
        //        return 0;
        //    }


        //    public void CheckPRCalculations(int layer)
        //    {
        //        IWebElement odyElement = driver.FindElement(calculatedOdyRiskLimit);
        //        int odyRiskActual = int.Parse(odyElement.Text);
        //        Assert.AreEqual(odyRiskLimitResult(layer), odyRiskActual);

        //        Assert.IsTrue(string.IsNullOrEmpty(odyRiskLimit.GetAttribute("value")));



        //        int valueRiskLimit = GetRiskLimitValue(layer);
        //        int valueSignedShare = GetSignedShareValue(layer);
        //        IWebElement odyRiskLimit = driver.FindElement(calculatedOdyRiskLimit);
        //        if (valueRiskLimit != 0 && valueSignedShare != 0)
        //        {
        //            int odyRiskResult = valueRiskLimit * valueSignedShare / 100;
        //            int odyRiskActual = int.Parse(odyRiskLimit.Text);
        //            Assert.AreEqual(odyRiskResult, odyRiskActual);
        //        }
        //        else
        //        {
        //            Assert.IsTrue(string.IsNullOrEmpty(odyRiskLimit.GetAttribute("value")));
        //        }

        //    }



        //    public void BoundLayer(CoverageType coverage, string[] values)
        //    {
        //        if (coverage == CoverageType.ProRata)
        //        {

        //        }

        //    }




        //}
    }
}
