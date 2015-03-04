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
using DiscoveryCenterTests.ObjectRepository;

namespace DiscoveryCenterTests
{
    /// <summary>
    /// Confirms that the user is able to create a new survey which saves to the database.
    /// </summary>
    [TestClass]
    public class AddSurveyTest : BaseTest
    {
        private string surveyName = Guid.NewGuid().ToString();
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
            this.ActiveBrowser.NavigateTo(Common.BaseUrl);

            SurveyListPage sListPage = new SurveyListPage(this);
            sListPage.AddSurvey.Click();

            EditSurveyPage editPage = new EditSurveyPage(this);
            Assert.IsTrue(editPage.TipDiv.InnerText.Contains("No Questions in survey."));
        }

        [TestMethod]
        public void AddDeleteQuestion()
        {
            this.ActiveBrowser.NavigateTo(Common.BaseUrl + "/Creation/Create");
            EditSurveyPage editPage = new EditSurveyPage(this);

            editPage.AddQuestion.Click();

            Assert.IsNotNull(editPage.GetDraggableQHeader(1));

            editPage.GetDeleteForQuestion(1).Click();

            Assert.IsTrue(editPage.TipDiv.InnerText.Contains("No Questions in survey."));
        }



        [TestMethod]
        public void ValidationErrors_EmptySurvey()
        {
            this.ActiveBrowser.NavigateTo(Common.BaseUrl + "/Creation/Create");
            EditSurveyPage editPage = new EditSurveyPage(this);

            //Click the save button while survey empty
            editPage.SaveButton.Click();

            //Assert that it doesnt save and validation summary is correct
            HtmlDiv divValidationSummary = editPage.ValidationSummary;
            Assert.IsTrue(divValidationSummary.InnerText.Contains("Survey contains errors."));
            Assert.IsTrue(divValidationSummary.InnerText.Contains("A survey name is required"));
            Assert.IsTrue(divValidationSummary.InnerText.Contains("A survey requires at least one question."));

            //Now add a question and confirm that the related error is now gone
            editPage.AddQuestion.Click();
            editPage.SaveButton.Click();

            //Have to refind Element if modified since last use.
            divValidationSummary = editPage.ValidationSummary;
            Assert.IsTrue(divValidationSummary.InnerText.Contains("Survey contains errors."));
            Assert.IsTrue(divValidationSummary.InnerText.Contains("A survey name is required"));
            Assert.IsFalse(divValidationSummary.InnerText.Contains("A survey requires at least one question."));

            //Add a name to survey, confirm associated validation error is removed.
            string uniqueSurveyName = Guid.NewGuid().ToString();
            editPage.SurveyName.Text = uniqueSurveyName;
            editPage.SaveButton.Click();

            divValidationSummary = editPage.ValidationSummary;
            Assert.IsTrue(divValidationSummary.InnerText.Contains("Survey contains errors."));
            Assert.IsFalse(divValidationSummary.InnerText.Contains("A survey name is required"));
            Assert.IsFalse(divValidationSummary.InnerText.Contains("A survey requires at least one question."));
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
            Common.TruncateDbTables();
            ShutDown();
        }

        #endregion

    }
}
