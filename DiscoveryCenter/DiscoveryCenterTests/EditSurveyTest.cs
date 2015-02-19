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

namespace DiscoveryCenterTests
{
    /// <summary>
    /// Confirms that a user is able to edit an existing survey and save the changes to the db.
    /// </summary>
    [TestClass]
    public class EditSurveyTest : BaseTest
    {
        private static Guid surveyName = Guid.NewGuid();

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
            theSurvey = new Survey()
            {
                Name = surveyName.ToString(),
                CreateDate = DateTime.Now
            };

            Question sAnswer = new Question()
            {
                Text="This is a short answer.",
                Type = Question.QuestionType.ShortAnswer,
                IndexInSurvey = 1,
                ParentSurvey = theSurvey
            };

            Question mChoose1 = new Question()
            {
                Text = "This is a multiple choice choose one.",
                Type = Question.QuestionType.MultipleChoiceChooseOne,
                IndexInSurvey = 2,
                ParentSurvey = theSurvey
            };

            Question mChooseM = new Question()
            {
                Text = "This is a multiple choice choose many.",
                Type = Question.QuestionType.MultipleChoiceChooseMany,
                IndexInSurvey = 3,
                ParentSurvey = theSurvey
            };

            Question sSlider = new Question()
            {
                Text = "This is a slider.",
                Type = Question.QuestionType.Slider,
                IndexInSurvey = 4,
                ParentSurvey = theSurvey
            };

            theSurvey.Questions = new List<Question>();
            theSurvey.Questions.Add(sAnswer);
            theSurvey.Questions.Add(mChoose1);
            theSurvey.Questions.Add(mChooseM);
            theSurvey.Questions.Add(sSlider);

            dbContext.Surveys.Add(theSurvey);
            dbContext.SaveChanges();
        }


        // Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
            #region WebAii Initialization

            
            Settings settings = GetSettings();

            settings.Web.DefaultBrowser = BrowserType.Chrome;

            Initialize(settings);
            #endregion

            //
            // Place any additional initialization here
            //

        }
        /// <summary>
        /// Checks to see if created survey is in db before testing web app.
        /// This test must be first. The order of all other tests do not matter.
        /// </summary>
        [TestMethod]
        public void SurveyInDb()
        {
            Survey found = (from m in dbContext.Surveys where m.Name == surveyName.ToString() select m).SingleOrDefault();
            Assert.IsNotNull(found);
        }

        /// <summary>
        /// Confirms that clicking "Edit" from the survey list page brings you to the 
        /// appropriate edit survey page and it is properly filled in.
        /// </summary>
        [TestMethod]
        public void EditSurveyNavigation()
        {
            this.Manager.LaunchNewBrowser();
            this.ActiveBrowser.Window.Maximize();
            this.ActiveBrowser.NavigateTo("http://discovery.rh.rit.edu/Production");

            //Click to edit the newly added survey
            Element btnEdit = this.ActiveBrowser.Find.ById("edit_" + theSurvey.Id);
            this.ActiveBrowser.Actions.Click(btnEdit);

            HtmlInputText txtSurveyName = new HtmlInputText(this.ActiveBrowser.Find.ById("Name"));

            //Assert the Name of the survey is correct upon navigation
            Assert.AreEqual(theSurvey.Name, txtSurveyName.Text);
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

    }
}
