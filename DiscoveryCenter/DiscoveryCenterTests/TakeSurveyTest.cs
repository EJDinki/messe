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
    /// Summary description for TakeSurveyTest
    /// </summary>
    [TestClass]
    public class TakeSurveyTest : BaseTest
    {
        private static Survey theSurvey;
        private static SurveyContext dbContext = new SurveyContext();

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
            theSurvey = Common.AddSurveyToDB();
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
        public void ViewSurvey()
        {
            SurveyListPage sListPage = new SurveyListPage(this);
            sListPage.GetViewFor(theSurvey.Id).Click();

            HtmlDiv welcomeDiv = this.Find.ByAttributes<HtmlDiv>("class=jumbotron");
            Assert.IsTrue(welcomeDiv.InnerText.Contains(theSurvey.Name));

            SurveyTakePage sTakePage = new SurveyTakePage(this);
            sTakePage.StartSurvey.Click();

        }


        /// <summary>
        /// Follows the Happy Path of Taking and submitting a survey.
        /// Answers All Questions. Clicks Complete Survey at the end.
        /// Confirms that TextAreas,RadioButtons,CheckBoxes,Sliders are interactable.
        /// Confirms that a proper Submission is saved upon completion.
        /// </summary>
        [TestMethod]
        public void TakeAndSubmit()
        {
            this.ActiveBrowser.NavigateTo(Common.BaseUrl + "/Home/Survey/" + theSurvey.Id);
            SurveyTakePage sTakePage = new SurveyTakePage(this);

            sTakePage.GetShortAnswerArea().Text = "ShortAnswer:" + theSurvey.Name;
            sTakePage.NextQuestion.Click();

            sTakePage.GetRadioButtonAtIndex(0).Check(true, true);
            sTakePage.GetRadioButtonAtIndex(1).Check(true, true);
            sTakePage.NextQuestion.Click();

            sTakePage.GetCheckBoxAtIndex(0).Check(true, true);
            sTakePage.GetCheckBoxAtIndex(1).Check(true, true);
            sTakePage.NextQuestion.Click();

            sTakePage.GetSliderHandle().DragTo
                (OffsetReference.AbsoluteCenter, new System.Drawing.Point(),
                sTakePage.GetSliderTrack(), OffsetReference.LeftCenter, new System.Drawing.Point());


            DateTime submissionTime = DateTime.Now;

            sTakePage.SubmitSurvey.Click();
            Assert.IsTrue(sTakePage.ThankYouDiv.InnerText.Contains("Thank you for taking our survey!"));

            Submission sub = (from s in dbContext.Submissions where s.SurveyId == theSurvey.Id select s).SingleOrDefault();

            //Assert All answers chosen were recorded in the appropriate submission
            Assert.AreEqual("ShortAnswer:" + theSurvey.Name,sub.Answers[0].Value);
            Assert.AreEqual("m1Choice2", sub.Answers[1].Value);
            Assert.AreEqual("mMChoice", sub.Answers[2].Value);
            Assert.AreEqual("mMChoice2", sub.Answers[3].Value);
            Assert.AreEqual("1", sub.Answers[4].Value);
        }
        #endregion
    }
}
