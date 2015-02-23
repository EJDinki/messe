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
    /// Summary description for CommonTests
    /// </summary>
    [TestClass]
    public class CommonTests : BaseTest
    {
        private string correctUser = "admin";
        private string correctPass = "admin101";

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
            settings.Web.RecycleBrowser = false;
            settings.ExecutionDelay = 100;

            Initialize(settings);
            #endregion

            Manager.LaunchNewBrowser();
            ActiveBrowser.Window.Maximize();
            ActiveBrowser.NavigateTo(Common.BaseUrl + "/Account/Login");

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
        public void LoginWrongUser()
        {
            LoginPage lPage = new LoginPage(this);

            lPage.Username.Text = "Wrong";
            lPage.Password.Text = correctPass;
            lPage.LogIn.Click();

            Assert.IsTrue(lPage.ValidationSummary.InnerText.Contains("Invalid username or password."));
        }

        [TestMethod]
        public void LoginWrongPassword()
        {
            LoginPage lPage = new LoginPage(this);

            lPage.Username.Text = correctUser;
            lPage.Password.Text = "Wrong";
            lPage.LogIn.Click();

            Assert.IsTrue(lPage.ValidationSummary.InnerText.Contains("Invalid username or password."));
        }

        [TestMethod]
        public void LoginCorrect()
        {
            LoginPage lPage = new LoginPage(this);

            lPage.Username.Text = correctUser;
            lPage.Password.Text = correctPass;
            lPage.LogIn.Click();

            SurveyListPage sListPage = new SurveyListPage(this);

            Assert.IsNotNull(sListPage.AddSurvey);
        }
        #endregion
    }
}
