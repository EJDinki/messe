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
using DiscoveryCenter.Models;
using DiscoveryCenterTests.ObjectRepository;
using ArtOfTest.Common;

namespace DiscoveryCenterTests
{
    /// <summary>
    /// Confirms that a user is able to edit an existing survey and save the changes to the db.
    /// </summary>
    [TestClass]
    public class EditSurveyTest : BaseTest
    {
        private static string surveyName;

        #region [Setup / TearDown]

        private static SurveyContext dbContext = new SurveyContext();
        private static Survey theSurvey;

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
            theSurvey = Common.AddSurveyToDB();
            dbContext.Set<Survey>().Attach(theSurvey);
            surveyName = theSurvey.Name;
        }


        // Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
            #region WebAii Initialization

            
            Settings settings = GetSettings();

            settings.Web.DefaultBrowser = BrowserType.Chrome;
            settings.Web.RecycleBrowser = false;
            settings.ExecutionDelay = 200;
            Initialize(settings);
            #endregion

            this.Manager.LaunchNewBrowser();
            this.ActiveBrowser.Window.Maximize();
            this.ActiveBrowser.NavigateTo(Common.BaseUrl);

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
            // This will shut down all browsers if
            // recycleBrowser is turned on. Else
            // will do nothing.
            ShutDown();
        }

        #endregion

        #region [Tests]

        [TestMethod]
        public void DuplicateSurvey()
        {
            SurveyListPage sListPage = new SurveyListPage(this);

            sListPage.GetSurveyRow(theSurvey.Id).MouseClick();
            sListPage.DuplicateSurvey.Click();

            //Assert Change is shown in web browser
            string copyName = theSurvey.Name + " - Copy";
            Assert.IsTrue(ActiveBrowser.ContainsText(copyName));

            //Assert the duplicate is in the database
            Survey copy = (from s in dbContext.Surveys where s.Name == copyName select s).SingleOrDefault();
            Assert.IsNotNull(copy);

        }

        /// <summary>
        /// Confirms that clicking "Edit" from the survey list page brings you to the 
        /// appropriate edit survey page and it is properly filled in.
        /// </summary>
        [TestMethod]
        public void EditSurveyNavigation()
        {
            //Click to edit the newly added survey
            SurveyListPage sListPage = new SurveyListPage(this);

            sListPage.GetSurveyRow(theSurvey.Id).MouseClick();
            sListPage.EditSurvey.Click();

            EditSurveyPage editPage = new EditSurveyPage(this);

            //Assert the Name of the survey is correct upon navigation
            Assert.AreEqual(theSurvey.Name, editPage.SurveyName.Text);
        }

        /// <summary>
        /// Confirms that reordering questions updates and saves appropriately
        /// </summary>
        [TestMethod]
        public void ReorderQuestions()
        {
            this.ActiveBrowser.NavigateTo(Common.BaseUrl+"/Creation/Edit/" + theSurvey.Id);

            string q1 = (from q in theSurvey.Questions where q.IndexInSurvey == 1 select q.Text)
                            .SingleOrDefault();

            string q2 = (from q in theSurvey.Questions where q.IndexInSurvey == 2 select q.Text)
                            .SingleOrDefault();

            EditSurveyPage editPage = new EditSurveyPage(this);

            HtmlInputText question1Text = 
                new HtmlInputText(editPage.GetQBody(1).Find.ById("~_Text"));

            //Assert the question text shown is equal to what is in the DB before the reorder
            Assert.IsTrue(editPage.GetDraggableQHeader(1).InnerText.Contains("Question1"));
            Assert.AreEqual(q1, question1Text.Text);

            //Reorder Questions
            editPage.GetDraggableQHeader(1).DragTo
                (OffsetReference.TopLeftCorner, new System.Drawing.Point(),
                editPage.GetQBody(2), OffsetReference.BottomLeftCorner, new System.Drawing.Point());


            //Refresh and check after drag
            question1Text = new HtmlInputText(editPage.GetQBody(1).Find.ById("~_Text"));
            

            //Assert the question text shown for question1 now shows what the database says is question 2
            Assert.IsTrue(editPage.GetDraggableQHeader(1).InnerText.Contains("Question1"));
            Assert.AreEqual(q2, question1Text.Text);

            editPage.SaveButton.Click();

            //Refresh Entities from db
            dbContext.Entry(theSurvey).Reload();
            foreach (Question q in theSurvey.Questions)
            {
                dbContext.Entry(q).Reload();
            }

            //Assert that the reorder was saved to the database correctly
            string newQ1 = (from q in theSurvey.Questions where q.IndexInSurvey == 1 select q.Text)
                            .SingleOrDefault();
            string newQ2 = (from q in theSurvey.Questions where q.IndexInSurvey == 2 select q.Text)
                            .SingleOrDefault();

            Assert.AreEqual(q2, newQ1);
            Assert.AreEqual(q1, newQ2);        
        }

        #endregion

        #region [HelperMethods]

        #endregion
    }
}
