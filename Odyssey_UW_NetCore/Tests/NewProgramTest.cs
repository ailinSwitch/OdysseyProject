using NUnit.Framework;
using Odyssey_UW.Pages;

namespace Odyssey_UW.Tests
{
    [TestFixture]
    class NewProgramTest : BaseTest
    {
        SearchPage searchPage;
        NewProgramPage newProgram;


        [TestCase("cSharp PR Test", "Greystone", " Toronto", " London MAS", "PENWALT CORPORATION", "UNITED ASN SVS", "Alberto", "PR", @"Surplus/",
            "March", "2016", "15")]
        public void createProgram(string programName, string assComp, string branch, string bussUnit, string cedant, string broker, string leadUW,
            string contractType, string treatyType, string month, string year, string day)
        {
            string[] selectDropValues = { assComp, branch, bussUnit };
            string[] typeAheadDropValues = { treatyType, leadUW };
            int yearInt = int.Parse(year);
            int dayInt = int.Parse(day);

            searchPage = new SearchPage(getDriver());
            //newProgram = searchPage.ClickNewProgramButton();
            //newProgram.SetProgramName(programName);
            //newProgram.SetSelectDpd(selectDropValues);
            //newProgram.SetTypeAheadDpd(typeAheadDropValues);
            //newProgram.SetContractType(contractType);
            //newProgram.SetEffDate(month, dayInt, yearInt);
            //newProgram.VerifyExpDateDisplayed();
            //newProgram.SetCedantAndBroker(cedant, broker);

            //if (!newProgram.IsCedantEnabled())
            //{
            //    Assert.AreEqual(cedant, newProgram.GetCedantSelected());
            //}
            //else
            //{
            //    Assert.Fail("The field is enabled. Should be disabled after select a value ");
            //}
            //if (!newProgram.IsBrokerEnabled())
            //{
            //    Assert.AreEqual(broker, newProgram.GetBrokerSelected());
            //}
            //else
            //{
            //    Assert.Fail("The field is enabled. Should be disabled after select a value ");
            //}

            //Console.WriteLine("Branch {0}, Broker {1}, Cedant {2}, Business Unit {3}, Treaty Type {4}, Underwriter {5}", newProgram.GetBranchSelected(), 
            //    newProgram.GetBrokerSelected(), 
            //    newProgram.GetCedantSelected(), newProgram.GetBunitSelected(), newProgram.GetTreatyTypeSelected(), newProgram.GetUWSelected());

            //newProgram.GetBranchSelected();
            //newProgram.ClickCreateBtn();
            searchPage.validateCreationByRecentPrograms(programName);




        }

    }
}
