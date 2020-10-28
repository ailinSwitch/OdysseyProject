using NUnit.Framework;
using Odyssey_UW.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Odyssey_UW.Tests
{
    [TestFixture]
    class EditProgramTest : BaseTest
    {
        SearchPage searchPage;


        [Test]
        public void checkAllPrograms()
        {
            searchPage = new SearchPage(driver);
            searchPage.checkAllPrograms();
        }
    }
}
