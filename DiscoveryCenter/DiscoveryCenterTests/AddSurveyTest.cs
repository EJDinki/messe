using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArtOfTest.WebAii.Controls.HtmlControls;
using ArtOfTest.WebAii.Controls.HtmlControls.HtmlAsserts;
using ArtOfTest.WebAii.Core;
using ArtOfTest.WebAii.ObjectModel;
using ArtOfTest.WebAii.TestAttributes;
using ArtOfTest.WebAii.TestTemplates;
using ArtOfTest.WebAii.Win32.Dialogs;

using ArtOfTest.WebAii.Silverlight;
using ArtOfTest.WebAii.Silverlight.UI;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DiscoveryCenterTests
{
    /// <summary>
    /// Confirms that the user is able to create a new survey which saves to the database.
    /// </summary>
    [TestClass]
    public class AddSurveyTest : BaseTest
    {

        #region [Setup / TearDown]

        private TestContext testContextInstance = null;
        /// <summary>
        ///Gets or sets the VS test context which provides
        ///information about and functionality for the
        ///current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }


        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
        }


        // Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
            #region WebAii Initialization
            Settings settings = GetSettings();
            settings.Web.DefaultBrowser = BrowserType.Chrome;
            settings.Web.RecycleBrowser = true;

            Initialize(settings);

            this.Manager.LaunchNewBrowser();
            this.ActiveBrowser.Window.Maximize();
            #endregion



        }

        [TestMethod]
        public void AddSurveyNavigation()
        {
            this.ActiveBrowser.NavigateTo("http://discovery.rh.rit.edu/Production");
            Element btnAddSurvey = this.ActiveBrowser.Find.ById("addItem");

            this.ActiveBrowser.Actions.Click(btnAddSurvey);

            Element divTip = this.ActiveBrowser.Find.ById("tip");
            Assert.IsTrue(divTip.InnerText.Contains("No Questions in survey."));
        }

        [TestMethod]
        public void ValidationErrors_EmptySurvey()
        {
            this.ActiveBrowser.NavigateTo("http://discovery.rh.rit.edu/Production/Creation/Create");
            Element btnSaveSurvey = this.ActiveBrowser.Find.ById("save");

            //Click the save button while survey empty
            this.ActiveBrowser.Actions.Click(btnSaveSurvey);

            //Assert that it doesnt save and validation summary is correct
            Element divValidationSummary = this.ActiveBrowser.Find.ById("valSum");
            Assert.IsTrue(divValidationSummary.InnerText.Contains("Survey contains errors."));
            Assert.IsTrue(divValidationSummary.InnerText.Contains("A survey name is required"));
            Assert.IsTrue(divValidationSummary.InnerText.Contains("Survey requires at least one question."));

            //Now add a question and confirm that the related error is now gone
            Element btnAddQuestion = this.ActiveBrowser.Find.ById("addItem");
            this.ActiveBrowser.Actions.Click(btnAddQuestion);
            this.ActiveBrowser.Actions.Click(btnSaveSurvey);

            //Have to refind Element if modified since last use.
            divValidationSummary = this.ActiveBrowser.Find.ById("valSum");
            Assert.IsTrue(divValidationSummary.InnerText.Contains("Survey contains errors."));
            Assert.IsTrue(divValidationSummary.InnerText.Contains("A survey name is required"));
            Assert.IsFalse(divValidationSummary.InnerText.Contains("Survey requires at least one question."));

            //Add a name to survey, confirm associated validation error is removed.
            Element txtSurveyName = this.ActiveBrowser.Find.ById("Name");
            Guid uniqueSurveyName = Guid.NewGuid();
            this.ActiveBrowser.Actions.SetText(txtSurveyName, uniqueSurveyName.ToString());
            this.ActiveBrowser.Actions.Click(btnSaveSurvey);

            divValidationSummary = this.ActiveBrowser.Find.ById("valSum");
            Assert.IsTrue(divValidationSummary.InnerText.Contains("Survey contains errors."));
            Assert.IsFalse(divValidationSummary.InnerText.Contains("A survey name is required"));
            Assert.IsFalse(divValidationSummary.InnerText.Contains("Survey requires at least one question."));
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {

            //
            // Place any additional cleanup here
            //

            #region WebAii CleanUp

            // Shuts down WebAii manager and closes all browsers currently running
            // after each test. This call is ignored if recycleBrowser is set
            this.CleanUp();

            #endregion
        }

        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            ShutDown();
        }

        #endregion

    }
}
